using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Travel_psw.Models;
using Travel_psw.Services;

namespace Travel_psw.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        // GET: api/admin/malicious-users
        [HttpGet("malicious-users")]
        public async Task<IActionResult> GetMaliciousUsers()
        {
            var users = await _adminService.GetMaliciousUsersAsync();
            return Ok(users);
        }

        // POST: api/admin/block-user/{id}
        [HttpPost("block-user/{id}")]
        public async Task<IActionResult> BlockUser(int id)
        {
            try
            {
                await _adminService.BlockUserAsync(id);
                return Ok("User has been blocked successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/admin/unblock-user/{id}
        [HttpPost("unblock-user/{id}")]
        public async Task<IActionResult> UnblockUser(int id)
        {
            try
            {
                await _adminService.UnblockUserAsync(id);
                return Ok(new { message = "User has been blocked successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/admin/update-status/{id}
        [HttpPut("update-status/{id}")]
        public async Task<IActionResult> UpdateUserStatus(int id)
        {
            try
            {
                await _adminService.UpdateUserStatusAsync(id);
                return Ok(new { message = "User has been unblocked successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
