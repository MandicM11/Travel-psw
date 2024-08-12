using Travel_psw.Data;
using Travel_psw.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


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
        _context.Tours.Add(tour);
        await _context.SaveChangesAsync();
        return tour;
    }

    public async Task<KeyPoint> AddKeyPointAsync(int tourId, KeyPoint keyPoint, IFormFile? image)
    {
        var tour = await _context.Tours.FindAsync(tourId);

        if (tour == null || tour.Status == "published")
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

        if (tour == null || tour.Status == "published")
            throw new InvalidOperationException("Tour not found or already published");

        if (tour.KeyPoints.Count < 2)
            throw new InvalidOperationException("Tour must have at least two key points to be published");

        tour.Status = "published";
        await _context.SaveChangesAsync();
    }
}
