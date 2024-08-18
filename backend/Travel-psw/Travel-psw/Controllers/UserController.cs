using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Travel_psw.Data;
using Travel_psw.Models;
using Travel_psw.Services;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly AwardService _awardService;
    private readonly RecommendationService _recommendationService;
    private readonly UserService _userService;

    public UsersController(AwardService awardService, RecommendationService recommendationService, UserService userService)
    {
        _awardService = awardService;
        _recommendationService = recommendationService;
        _userService = userService;
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

    [HttpPost("recommendations")]
    public IActionResult RequestRecommendations([FromQuery] string difficulty)
    {
        var user = _userService.GetLoggedInUser();

        if (user == null)
        {
            return Unauthorized("User is not logged in.");
        }

        _recommendationService.SendRecommendationsByEmail(user, difficulty);

        return Ok("Recommendations have been sent to your email.");
    }

    [HttpGet("recommendations")]
    public IActionResult GetRecommendations([FromQuery] string difficulty)
    {
        var user = _userService.GetLoggedInUser();

        if (user == null)
        {
            return Unauthorized("User is not logged in.");
        }

        var recommendations = _recommendationService.GetRecommendations(user, difficulty);

        if (recommendations == null || !recommendations.Any())
        {
            return NotFound("No tours found matching your interests and selected difficulty.");
        }

        return Ok(recommendations);
    }

}
