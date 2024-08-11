using Microsoft.AspNetCore.Mvc;
using Travel_psw.Models;
using Travel_psw.Services;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;

namespace Travel_psw.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Travel_psw.Models.LoginRequest loginModel)
        {
            var user = await _userService.AuthenticateUserAsync(loginModel.Username, loginModel.Password);
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            // Generate JWT
            var token = _userService.GenerateJwtToken(user);

            return Ok(new { token });
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Simply invalidate the token client-side by removing it.
            return Ok(new { message = "Logged out successfully" });
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
