using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Travel_psw.Models;
using System.Collections.Generic;
using Travel_psw.Data;

namespace Travel_psw.Services
{
    public class CartService
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public CartService(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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

            var emailBody = $"Thank you for your purchase! You have bought the following tours:\n\n" +
                            string.Join("\n", cart.Items.Select(i => $"{i.Tour.Title} (Quantity: {i.Quantity})")) +
                            $"\n\nTotal Price: {cart.TotalPrice:C}";

            // Pošaljite potvrdu putem emaila
            await _emailService.SendEmailAsync(cart.User.Email, "Purchase Confirmation", emailBody);

            // Očistite korpu nakon potvrde
            cart.Items.Clear();
            cart.TotalPrice = 0;
            await _context.SaveChangesAsync();
        }


        public async Task<Tour> GetTourById(int tourId)
        {
            return await _context.Tours
                .FirstOrDefaultAsync(t => t.Id == tourId);
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
