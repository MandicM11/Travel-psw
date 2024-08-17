using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Travel_psw.Data;
using Travel_psw.Models;

public interface ISaleRepository
{
    Task<List<Sale>> GetSalesByDateRange(int userId, DateTime startDate, DateTime endDate);
    Task<List<Sale>> GetSalesForLastThreeMonthsAsync(int tourId, DateTime startDate);
    Task AddSaleAsync(Sale sale);
    Task<Sale> GetSaleByIdAsync(int id);
}

public class SaleRepository : ISaleRepository
{
    private readonly ApplicationDbContext _context;

    public SaleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Sale>> GetSalesByDateRange(int userId, DateTime startDate, DateTime endDate)
    {
        return await _context.Sales
            .Where(s => s.UserId == userId && s.SaleDate >= startDate && s.SaleDate <= endDate)
            .ToListAsync();
    }

    public async Task AddSaleAsync(Sale sale)
    {
        _context.Sales.Add(sale);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Sale>> GetSalesForLastThreeMonthsAsync(int tourId, DateTime startDate)
    {
        var threeMonthsAgo = startDate.AddMonths(-3);

        return await _context.Sales
            .Where(s => s.TourId == tourId && s.SaleDate >= threeMonthsAgo && s.SaleDate <= startDate)
            .ToListAsync();
    }

    public async Task<Sale> GetSaleByIdAsync(int id) // Dodajte ovu metodu
    {
        return await _context.Sales.FindAsync(id);
    }

}
