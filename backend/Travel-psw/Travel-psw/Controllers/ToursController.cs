using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Travel_psw.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using Travel_psw.Services;

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
    public async Task<IActionResult> CreateTour([FromBody] TourDTO tourDTO)
    {
        if (tourDTO == null)
            return BadRequest("Tour object is null");

        var tour = new Tour
        {
            Title = tourDTO.Title,
            Description = tourDTO.Description,
            Difficulty = tourDTO.Difficulty,
            Category = tourDTO.Category,
            Price = tourDTO.Price,
            Status = tourDTO.Status,
            AuthorId = tourDTO.AuthorId
        };

        try
        {
            var createdTour = await _tourService.CreateTour(tour);
            return CreatedAtAction(nameof(GetTourById), new { id = createdTour.Id }, createdTour);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
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
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetTours([FromQuery] TourStatus? status, [FromQuery] bool? isRewarded)
    {
        var tours = await _tourService.GetToursByStatusAsync(status, isRewarded);
        return Ok(tours);
    }

    [HttpGet("author/{authorId}")]
    public async Task<IActionResult> GetToursByAuthor(int authorId)
    {
        try
        {
            var tours = await _tourService.GetToursByAuthorAsync(authorId);
            if (tours == null || !tours.Any())
                return NotFound();

            return Ok(tours);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
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

    [HttpGet("report")]
    public async Task<IActionResult> GenerateReport([FromQuery] DateTime month)
    {
        try
        {
            var report = await _tourService.GenerateMonthlyReport(month);
            if (report == null)
                return NotFound();

            return Ok(report);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
