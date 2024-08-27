using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Travel_psw.Data;
using Travel_psw.Models;
using System.Collections.Generic;
using System.Linq;
using System;

public class TourService
{
    private readonly ApplicationDbContext _context;

    public TourService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Tour?> GetTourByIdAsync(int id)
    {
        Console.WriteLine($"Fetching tour with ID {id}...");
        var tour = await _context.Tours
            .Include(t => t.KeyPoints)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tour == null)
        {
            Console.WriteLine($"Tour with ID {id} not found.");
        }
        else
        {
            Console.WriteLine($"Tour with ID {id} found.");
        }

        return tour;
    }

    public async Task<Tour> CreateTour(Tour tour)
    {
        // Validiraj AuthorId
        var author = await _context.Users.FindAsync(tour.AuthorId);
        if (author == null)
        {
            throw new ArgumentException("Invalid AuthorId");
        }

        // Postavi Author objekat
        tour.Author = author;

        _context.Tours.Add(tour);
        await _context.SaveChangesAsync();

        return tour;
    }

    public async Task<List<Tour>> GetPurchasedToursByUserAsync(int userId)
    {
        return await _context.Tours
            .Where(t => t.Purchases.Any(p => p.UserId == userId))
            .ToListAsync();
    }






    public async Task<KeyPoint> AddKeyPointAsync(int tourId, KeyPoint keyPoint, IFormFile? image)
    {
        var tour = await _context.Tours.FindAsync(tourId);

        if (tour == null || tour.Status == TourStatus.Published)
            throw new InvalidOperationException("Tour not found or already published");

        if (image != null && image.Length > 0)
        {
            // Postavljanje putanje za folder za upload
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
            var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            // Kreiranje foldera ako ne postoji
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Čuvanje slike
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            // Čuvanje putanje slike u KeyPoint modelu
            keyPoint.ImageUrl = fileName; // Ovdje postavljaš ime datoteke
        }
        else
        {
            // Ako slika nije pružena, postavi `ImageUrl` kao null ili ne postavljaj ništa
            keyPoint.ImageUrl = null;
        }

        keyPoint.TourId = tourId;
        _context.KeyPoints.Add(keyPoint);
        await _context.SaveChangesAsync();

        return keyPoint;
    }

    public async Task PublishTour(int tourId)
    {
        var tour = await _context.Tours.Include(t => t.KeyPoints).FirstOrDefaultAsync(t => t.Id == tourId);

        if (tour == null || tour.Status == TourStatus.Published)
            throw new InvalidOperationException("Tour not found or already published");

        if (tour.KeyPoints.Count < 2)
            throw new InvalidOperationException("Tour must have at least two key points to be published");

        tour.Status = TourStatus.Published;
        await _context.SaveChangesAsync();
    }

    public async Task ArchiveTourAsync(int tourId)
    {
        var tour = await _context.Tours.FindAsync(tourId);

        if (tour == null)
            throw new InvalidOperationException("Tour not found");

        if (tour.Status == TourStatus.Archived)
            throw new InvalidOperationException("Tour is already archived");

        tour.Status = TourStatus.Archived;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Tour>> GetToursByStatusAsync(TourStatus? status = null, bool? isRewarded = null)
    {
        var query = _context.Tours.Include(t => t.KeyPoints).AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        if (isRewarded.HasValue)
        {
            query = query.Where(t => t.Author.IsAwarded == isRewarded.Value);
        }
        return await query.ToListAsync();
    }

    /*public async Task<List<Tour>> GetToursByAwardAsync(string status = null, bool? isRewarded = null)
    {
        var query = _context.Tours.AsQueryable();


        if (isRewarded.HasValue)
        {
            query = query.Where(t => t.Author.IsAwarded == isRewarded.Value);
        }

        return await query.ToListAsync();
    }*/


    public async Task<IEnumerable<Tour>> GetToursByAuthorAsync(int authorId)
    {
        return await _context.Tours
            .Where(t => t.AuthorId == authorId)
            .Include(t => t.KeyPoints)
            .ToListAsync();
    }

    public async Task<IEnumerable<KeyPoint>> GetKeyPointsAsync(int tourId)
    {
        var tour = await _context.Tours
            .Include(t => t.KeyPoints)
            .FirstOrDefaultAsync(t => t.Id == tourId);

        if (tour == null)
        {
            throw new InvalidOperationException("Tour not found");
        }

        return tour.KeyPoints;
    }

    public async Task<ReportDto> GenerateMonthlyReport(DateTime month)
    {
        var startDate = new DateTime(month.Year, month.Month, 1);
        var endDate = startDate.AddMonths(1).AddTicks(-1);

        var tours = await _context.Tours
            .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate)
            .ToListAsync();

        var sales = await _context.Sales
            .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
            .ToListAsync();

        return new ReportDto
        {
            ToursCreated = tours.Count,
            TotalSales = sales.Sum(s => s.Amount),
            MostPopularTour = tours.OrderByDescending(t => t.Sales.Count).FirstOrDefault()?.Title
        };
    }
}
