using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Travel_psw.Models;


[Route("api/[controller]")]
[ApiController]
public class ToursController : ControllerBase
{
    private readonly TourService _tourService;

    public ToursController(TourService tourService)
    {
        _tourService = tourService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTour([FromBody] Tour tour)
    {
        var createdTour = await _tourService.CreateTour(tour);
        return CreatedAtAction(nameof(GetTourById), new { id = createdTour.Id }, createdTour);
    }

    [HttpPost("{tourId}/keypoints")]
    public async Task<IActionResult> AddKeyPoint(int tourId, [FromForm] KeyPoint keyPoint, IFormFile? image)
    {
        try
        {
            var createdKeyPoint = await _tourService.AddKeyPointAsync(tourId, keyPoint, image);
            return CreatedAtAction(nameof(GetTourById), new { id = tourId }, createdKeyPoint);
        }
        catch (Exception ex)
        {
            // Obavi odgovarajuće obrade greške
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }






    [HttpPost("{tourId}/publish")]
    public async Task<IActionResult> PublishTour(int tourId)
    {
        await _tourService.PublishTour(tourId);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTourById(int id)
    {
        var tour = await _tourService.GetTourByIdAsync(id);
        if (tour == null)
            return NotFound();

        return Ok(tour);
    }
}
