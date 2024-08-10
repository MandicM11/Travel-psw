namespace Travel_psw.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Travel_psw.Models;
    

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            // Logika za registraciju korisnika, kao što je validacija i dodavanje u bazu podataka
            // Ovdje ćemo samo vratiti potvrdu registracije
            return Ok("User registered successfully");
        }
    }


}
