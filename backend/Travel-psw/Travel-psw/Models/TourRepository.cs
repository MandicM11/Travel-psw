using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Travel_psw.Data;
using Travel_psw.Models;

public interface ITourRepository
{
    Task<List<Tour>> GetToursByDateRange(DateTime startDate, DateTime endDate);
    Task<Tour> GetTourByIdAsync(int tourId);
    Task UpdateTourAsync(Tour tour);
    Task<List<Tour>> FindByConditionAsync(Expression<Func<Tour, bool>> expression);
}

public class TourRepository : ITourRepository
{
    private readonly ApplicationDbContext _context;

    public TourRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Tour>> GetToursByDateRange(DateTime startDate, DateTime endDate)
    {
        return await _context.Tours
            .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate)
            .ToListAsync();
    }

    public async Task<Tour> GetTourByIdAsync(int tourId)
    {
        return await _context.Tours.FindAsync(tourId);
    }

    public async Task UpdateTourAsync(Tour tour)
    {
        _context.Tours.Update(tour);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Tour>> FindByConditionAsync(Expression<Func<Tour, bool>> expression)
    {
        return await _context.Tours
            .Where(expression)
            .ToListAsync();
    }
}
