using Microsoft.AspNetCore.Mvc;
using Travel_psw.Services;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly AwardService _awardService;

    public UsersController(AwardService awardService)
    {
        _awardService = awardService;
    }

    [HttpGet("awarded-authors")]
    public async Task<IActionResult> GetAwardedAuthors()
    {
        var awardedAuthors = await _awardService.GetAwardedAuthors();
        if (awardedAuthors == null || !awardedAuthors.Any())
        {
            return NotFound("No awarded authors found.");
        }

        return Ok(awardedAuthors);
    }
}
