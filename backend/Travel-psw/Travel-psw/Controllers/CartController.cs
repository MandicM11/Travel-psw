using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travel_psw.Models;
using Travel_psw.Services;

namespace Travel_psw.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
public class CartController: ControllerBase
    {
        private readonly CartService _cartService;
        private readonly EmailService _emailService;
        private readonly ITourRepository _tourRepository;
        private readonly ISaleRepository _saleRepository;

        public CartController(
            CartService cartService,
            EmailService emailService,
            ITourRepository tourRepository,
            ISaleRepository saleRepository)
        {
            _cartService = cartService;
            _emailService = emailService;
            _tourRepository = tourRepository;
            _saleRepository = saleRepository;
        }

        [HttpPost("{cartId}/items")]
        public async Task<IActionResult> AddItemToCart(int cartId, [FromBody] CartItemDto itemDto)
        {
            if (itemDto == null || itemDto.TourId <= 0)
            {
                return BadRequest("Invalid item data.");
            }

            try
            {
                await _cartService.AddToCartAsync(cartId, itemDto.TourId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message); // Koristite status 409 Conflict za greške u dodavanju
            }
        }

        [HttpDelete("{cartId}/items/{tourId}")]
        public async Task<IActionResult> RemoveItemFromCart(int cartId, int tourId)
        {
            try
            {
                await _cartService.RemoveFromCartAsync(cartId, tourId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetCart(int cartId)
        {
            var cart = await _cartService.GetCartAsync(cartId);
            if (cart == null)
            {
                return NotFound("Cart not found.");
            }

            return Ok(cart);
        }

        [HttpPost("{cartId}/confirm")]
        public async Task<IActionResult> ConfirmPurchase(int cartId)
        {
            try
            {
                var cart = await _cartService.GetCartAsync(cartId);
                if (cart == null)
                {
                    return NotFound("Cart not found.");
                }

                foreach (var item in cart.Items)
                {
                    var sale = new Sale
                    {
                        TourId = item.TourId,
                        Amount = item.Quantity * item.Tour.Price,
                        UserId = cart.UserId,
                        SaleDate = DateTime.UtcNow
                    };

                    await _saleRepository.AddSaleAsync(sale);
                }

                var tours = cart.Items.Select(i => new { i.Tour.Title, i.Quantity }).ToList();
                var emailBody = GenerateEmailBody(tours, cart.TotalPrice);
                await _emailService.SendEmailAsync(cart.User.Email, "Purchase Confirmation", emailBody);

                // Očisti korpu nakon potvrde
                cart.Items.Clear();
                cart.TotalPrice = 0;
                await _cartService.UpdateCartAsync(cart);

                return NoContent(); // Status 204 No Content za uspešnu operaciju
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error while processing purchase.");
            }
        }

        private string GenerateEmailBody(IEnumerable < dynamic > tours, decimal totalPrice)
        {
            var emailBody = $"Thank you for your purchase! You have bought the following tours:\n\n" +
                string.Join("\n", tours.Select(t => $"{t.Title} (Quantity: {t.Quantity})")) +
                $"\n\nTotal Price: {totalPrice:C}";

            return emailBody;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart([FromBody] CreateCartDto createCartDto)
        {
            if (createCartDto == null || createCartDto.UserId <= 0)
            {
                return BadRequest("Invalid cart data.");
            }

            try
            {
                var cart = await _cartService.CreateCartAsync(createCartDto.UserId);
                return CreatedAtAction(nameof(GetCart), new { cartId = cart.Id }, cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCartByUserId(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            try
            {
                var cart = await _cartService.GetCartByUserIdAsync(userId);
                if (cart == null)
                {
                    return NotFound("Cart not found for this user.");
                }

                return Ok(cart);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
