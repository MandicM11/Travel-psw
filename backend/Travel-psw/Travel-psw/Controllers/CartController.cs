using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travel_psw.Models;
using Travel_psw.Services;

namespace Travel_psw.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;
        private readonly EmailService _emailService;

        public CartController(CartService cartService, EmailService emailService)
        {
            _cartService = cartService;
            _emailService = emailService;
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
                return Conflict(ex.Message);  // Koristite status 409 Conflict za greške u dodavanju
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
            var cart = await _cartService.GetCartAsync(cartId);
            if (cart == null)
            {
                return NotFound("Cart not found.");
            }

            if (cart.User == null || string.IsNullOrEmpty(cart.User.Email))
            {
                return BadRequest("User email is not available.");
            }

            var tours = await Task.WhenAll(cart.Items.Select(async i => await _cartService.GetTourById(i.TourId)));
            var emailBody = GenerateEmailBody(tours);

            await _emailService.SendEmailAsync(cart.User.Email, "Purchase Confirmation", emailBody);

            cart.Items.Clear();
            cart.TotalPrice = 0;
            await _cartService.UpdateCartAsync(cart);

            return NoContent();
        }


        // Helper method to generate the email body
        private string GenerateEmailBody(IEnumerable<Tour> tours, decimal totalPrice)
        {
            var emailBody = "Thank you for your purchase! You have bought the following tours:\n\n" +
                             string.Join("\n", tours.Select(t => $"{t.Title} (Quantity: 1)")) +
                             $"\n\nTotal Price: {totalPrice:C}";
            return emailBody;
        }


        private string GenerateEmailBody(IEnumerable<Tour> tours)
        {
            return string.Join("<br>", tours.Select(t => $"{t.Title} - {t.Price:C}"));
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

        [HttpGet]
        public async Task<IActionResult> GetCartByUserId([FromQuery] int userId)
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
                    return NotFound("Cart not found.");
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
