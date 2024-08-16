using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travel_psw.Models;

public class ReportService
{
    private readonly ITourRepository _tourRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISaleRepository _saleRepository;

    public ReportService(ITourRepository tourRepository, IUserRepository userRepository, ISaleRepository saleRepository)
    {
        _tourRepository = tourRepository;
        _userRepository = userRepository;
        _saleRepository = saleRepository;
    }

    public async Task<Report> GenerateMonthlyReport(DateTime month)
    {
        var startOfMonth = new DateTime(month.Year, month.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1);

        var tours = await _tourRepository.GetToursByDateRange(startOfMonth, endOfMonth);
        var users = await _userRepository.GetAllUsers();

        var report = new List<UserReport>();

        foreach (var user in users)
        {
            var userTours = tours.Where(t => t.AuthorId == user.Id).ToList();
            var sales = await _saleRepository.GetSalesByDateRange(user.Id, startOfMonth, endOfMonth);

            var totalSales = sales.Sum(s => s.Amount);
            var totalTours = userTours.Count;

            var previousMonth = startOfMonth.AddMonths(-1);
            var previousMonthTours = await _tourRepository.GetToursByDateRange(
                new DateTime(previousMonth.Year, previousMonth.Month, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(previousMonth.Year, previousMonth.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1).AddTicks(-1)
            );

            var previousMonthUserTours = previousMonthTours.Where(t => t.AuthorId == user.Id).ToList();
            var previousSales = await _saleRepository.GetSalesByDateRange(user.Id, new DateTime(previousMonth.Year, previousMonth.Month, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(previousMonth.Year, previousMonth.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1).AddTicks(-1));
            var previousTotalSales = previousSales.Sum(s => s.Amount);

            var percentageChange = (previousTotalSales == 0)
                ? (totalSales > 0 ? 100 : 0)
                : (totalSales - previousTotalSales) / previousTotalSales * 100;

            var reportItem = new UserReport
            {
                UserId = user.Id,
                UserName = user.Username, // Pretpostavljam da je 'Username' ta svojstva
                TotalSales = totalSales,
                TotalTours = totalTours,
                PercentageChange = percentageChange,
                BestSellingTours = userTours
                                    .Where(t => sales.Any(s => s.TourId == t.Id))
                                    .OrderByDescending(t => sales.Count(s => s.TourId == t.Id))
                                    .Take(5)
                                    .ToList(),
                UnsoldTours = await GetUnsoldTours(user.Id, startOfMonth, endOfMonth),
            };

            report.Add(reportItem);
        }

        var summary = new ReportDto
        {
            ToursCreated = tours.Count,
            TotalSales = report.Sum(r => r.TotalSales),
            MostPopularTour = report.SelectMany(r => r.BestSellingTours)
                                      .GroupBy(t => t.Title)
                                      .OrderByDescending(g => g.Count())
                                      .FirstOrDefault()?.Key
        };

        return new Report
        {
            MonthlyReports = report,
            Summary = summary
        };
    }

    private async Task<List<Tour>> GetUnsoldTours(int userId, DateTime startOfMonth, DateTime endOfMonth)
    {
        var allTours = await _tourRepository.GetToursByDateRange(startOfMonth, endOfMonth);
        var sales = await _saleRepository.GetSalesByDateRange(userId, startOfMonth, endOfMonth);

        var soldTourIds = sales.Select(s => s.TourId).Distinct();

        return allTours.Where(t => t.AuthorId == userId && !soldTourIds.Contains(t.Id)).ToList();
    }

    public async Task ArchiveTours(int userId, List<int> tourIds)
    {
        foreach (var tourId in tourIds)
        {
            var tour = await _tourRepository.GetTourByIdAsync(tourId);
            if (tour != null)
            {
                tour.Status = TourStatus.Archived;
                await _tourRepository.UpdateTourAsync(tour);
            }
        }
    }
}
