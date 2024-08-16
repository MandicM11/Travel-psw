using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Travel_psw.Data;
using Travel_psw.Models;

public interface ISaleRepository
{
    Task<List<Sale>> GetSalesByDateRange(int userId, DateTime startDate, DateTime endDate);
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
}
