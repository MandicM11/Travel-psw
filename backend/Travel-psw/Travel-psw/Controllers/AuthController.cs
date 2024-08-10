using Microsoft.AspNetCore.Mvc;
using Travel_psw.Models;
using Travel_psw.Services;

namespace Travel_psw.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            // Validacija korisničkih podataka
            if (string.IsNullOrEmpty(user.Username) ||
                string.IsNullOrEmpty(user.Password) ||
                string.IsNullOrEmpty(user.FirstName) ||
                string.IsNullOrEmpty(user.LastName) ||
                string.IsNullOrEmpty(user.Email))
            {
                return BadRequest("All fields are required.");
            }

            // Proveri da li korisničko ime već postoji
            if (await _userService.UserExistsAsync(user.Username))
            {
                return Conflict("Username already exists.");
            }

            // Dodaj korisnika u bazu
            var newUser = await _userService.AddUserAsync(user);

            // Vraćanje uspešne poruke sa informacijama o korisniku
            return Ok(new { message = "User registered successfully", user = newUser });
        }
    }
}
