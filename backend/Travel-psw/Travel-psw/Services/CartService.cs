using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Travel_psw.Models;
using System.Collections.Generic;
using Travel_psw.Data;
using Travel_psw.Controllers;
using System.Net.Http;

namespace Travel_psw.Services
{
    public class CartService
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;
        private readonly ISaleRepository _saleRepository;
        private readonly  ILogger<CartService> _logger;
        private readonly HttpClient _httpClient;

        public CartService(ApplicationDbContext context, EmailService emailService, ISaleRepository saleRepository, ILogger<CartService> logger, HttpClient httpClient)
        {
            _context = context;
            _emailService = emailService;
            _saleRepository = saleRepository;
        }

        public async Task<Cart> GetCartAsync(int cartId)
        {
            return await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Tour) // Uključi ture
                .Include(c => c.User) // Uključi korisnika
                .FirstOrDefaultAsync(c => c.Id == cartId);
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            // Pretpostavljamo da koristite neki ORM kao što je Entity Framework
            return await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Tour)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task AddToCartAsync(int cartId, int tourId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart == null) throw new Exception("Cart not found");

            var tour = await _context.Tours.FindAsync(tourId);
            if (tour == null) throw new Exception("Tour not found");

            var existingItem = cart.Items.FirstOrDefault(i => i.TourId == tourId);
            if (existingItem != null)
            {
                // Ako je tura već u korpi, ne dodaj više od jednog primerka
                throw new Exception("This tour is already in the cart.");
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    CartId = cartId,
                    TourId = tourId,
                    Quantity = 1  // Uvek dodaj samo jedan primerak
                });
            }

            cart.TotalPrice = cart.Items.Sum(i => i.Quantity * _context.Tours.Find(i.TourId).Price);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(int cartId, int tourId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart == null) throw new Exception("Cart not found");

            var item = cart.Items.FirstOrDefault(i => i.TourId == tourId);
            if (item != null)
            {
                cart.Items.Remove(item);
                // Izračunaj ukupnu cenu koristeći cene tura
                cart.TotalPrice = await _context.CartItems
                    .Where(i => i.CartId == cartId)
                    .Include(i => i.Tour)
                    .SumAsync(i => i.Quantity * i.Tour.Price);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ConfirmPurchaseAsync(int cartId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                    .ThenInclude(ci => ci.Tour)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart == null)
                throw new Exception("Cart not found.");

            foreach (var item in cart.Items)
            {
                if (item.Tour == null)
                {
                    _logger.LogWarning("Tour for item with TourId {TourId} is not loaded.", item.TourId);
                    continue; // Preskoči ovaj item
                }

                
                _logger.LogInformation("Creating sale with TourId {TourId}, Quantity {Quantity}, Price {Price}, UserId {UserId}",
                    item.TourId, item.Quantity, item.Tour.Price, cart.UserId);

                var newSale = new Sale
                {
                    TourId = item.TourId,
                    Amount = item.Quantity * item.Tour.Price,
                    UserId = cart.UserId,
                    SaleDate = DateTime.UtcNow
                };

                try
                {
                    _logger.LogInformation("Creating sale: {@Sale}", newSale);
                    await _saleRepository.AddSaleAsync(newSale);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while adding sale in ConfirmPurchaseAsync");
                    throw;  // Ponovo baci izuzetak kako bi se obradio dalje
                }
            }

            // Očisti korpu nakon potvrde
            cart.Items.Clear();
            cart.TotalPrice = 0;
            await _context.SaveChangesAsync();
        }


        








        public async Task<Cart> CreateCartAsync(int userId)
        {
            var cart = new Cart
            {
                UserId = userId,
                TotalPrice = 0,
                CreatedAt = DateTime.UtcNow
            };

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return cart;
        }

    }
}
