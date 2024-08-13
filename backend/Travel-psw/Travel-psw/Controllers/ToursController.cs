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

    [HttpGet("{tourId}/keypoints")]
    public async Task<IActionResult> GetKeyPoints(int tourId)
    {
        try
        {
            var keyPoints = await _tourService.GetKeyPointsAsync(tourId);
            return Ok(keyPoints);
        }
        catch (Exception ex)
        {
            // Obavi odgovarajuće obrade greške
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }


    [HttpGet]
    public async Task<IActionResult> GetTours([FromQuery] TourStatus? status = null)
    {
        var tours = await _tourService.GetToursByStatusAsync(status);
        return Ok(tours);
    }



    [HttpPut("{tourId}/publish")]
    public async Task<IActionResult> PublishTour(int tourId)
    {
        try
        {
            await _tourService.PublishTour(tourId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }



    [HttpPut("{tourId}/archive")]
    public async Task<IActionResult> ArchiveTour(int tourId)
    {
        try
        {
            await _tourService.ArchiveTourAsync(tourId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
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
