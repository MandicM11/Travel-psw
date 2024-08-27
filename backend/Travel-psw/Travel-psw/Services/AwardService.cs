using Travel_psw.Data;
using Travel_psw.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Travel_psw.Services
{
    public class AwardService
    {
        private readonly ApplicationDbContext _context;

        public AwardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AwardAuthorsAsync()
        {
            // Izračunajte broj prodatih tura po autoru
            var sales = await _context.Sales
                .Where(s => s.SaleDate.Month == DateTime.Now.Month)
                .GroupBy(s => s.Tour.AuthorId)
                .Select(g => new
                {
                    AuthorId = g.Key,
                    SalesCount = g.Count()
                })
                .ToListAsync();

            foreach (var sale in sales)
            {
                var author = await _context.Users.FindAsync(sale.AuthorId);
                if (author != null && author.Role == UserRole.Author)
                {
                    author.Points += 1;
                    if (author.Points >= 5)
                    {
                        author.IsAwarded = true;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAwardedAuthors()
        {
            return await _context.Users
                .Where(u => u.Role == UserRole.Author && u.IsAwarded)
                .ToListAsync();
        }

        

    }

}
