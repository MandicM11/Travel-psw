using Microsoft.EntityFrameworkCore;
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
    private readonly EmailService _emailService;

    public ReportService(ITourRepository tourRepository, IUserRepository userRepository, ISaleRepository saleRepository, EmailService emailService)
    {
        _tourRepository = tourRepository ?? throw new ArgumentNullException(nameof(tourRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    }

    public async Task GenerateMonthlyReportAsync(DateTime reportDate)
    {
        // Konvertujte reportDate u UTC pre nego što koristite
        var startOfMonth = new DateTime(reportDate.Year, reportDate.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

        // Dobavljanje autora
        var authors = await _userRepository.GetAllUsers();

        foreach (var author in authors)
        {
            // 1. Izračunavanje prodaja za tekući mesec
            var salesThisMonth = await _saleRepository.GetSalesByDateRange(author.Id, startOfMonth, endOfMonth);

            var totalSalesThisMonth = salesThisMonth.Sum(s => s.Amount);
            var totalToursSoldThisMonth = salesThisMonth.GroupBy(s => s.TourId).Count();

            // 2. Izračunavanje prodaja za prethodni mesec
            var startOfPreviousMonth = startOfMonth.AddMonths(-1);
            var endOfPreviousMonth = endOfMonth.AddMonths(-1);

            var salesLastMonth = await _saleRepository.GetSalesByDateRange(author.Id, startOfPreviousMonth, endOfPreviousMonth);

            var totalSalesLastMonth = salesLastMonth.Sum(s => s.Amount);
            var totalToursSoldLastMonth = salesLastMonth.GroupBy(s => s.TourId).Count();

            // 3. Procenat porasta/opadanja
            decimal salesGrowthPercentage = 0;
            if (totalSalesLastMonth > 0)
            {
                salesGrowthPercentage = ((totalSalesThisMonth - totalSalesLastMonth) / totalSalesLastMonth) * 100;
            }

            // 4. Identifikovanje najprodavanijih i neprodatih tura
            var toursByAuthor = await _tourRepository.FindByConditionAsync(t => t.AuthorId == author.Id);

            var bestSellingTour = salesThisMonth
                .GroupBy(s => s.TourId)
                .OrderByDescending(g => g.Sum(s => s.Amount))
                .Select(g => g.Key)
                .FirstOrDefault();

            var unsoldToursThisMonth = toursByAuthor
                .Where(t => !salesThisMonth.Any(s => s.TourId == t.Id))
                .ToList();

            // Provera tura koje nisu prodate tri uzastopna meseca
            var unsoldForThreeMonths = new List<Tour>();
            foreach (var tour in unsoldToursThisMonth)
            {
                var salesInLastThreeMonths = await _saleRepository.GetSalesByDateRange(tour.Id, startOfMonth.AddMonths(-3), endOfMonth.AddMonths(-3));

                if (!salesInLastThreeMonths.Any())
                {
                    unsoldForThreeMonths.Add(tour);
                }
            }

            // 5. Slanje izveštaja putem emaila autoru
            var reportBody = $"Monthly Report for {startOfMonth:MMMM yyyy}\n" +
                             $"Total Tours Sold: {totalToursSoldThisMonth}\n" +
                             $"Total Revenue: {totalSalesThisMonth:C}\n" +
                             $"Sales Growth: {salesGrowthPercentage:+0.00%;-0.00%}\n\n" +
                             $"Best Selling Tour: {bestSellingTour}\n" +
                             $"Unsold Tours: {string.Join(", ", unsoldToursThisMonth.Select(t => t.Title))}\n";

            if (unsoldForThreeMonths.Any())
            {
                reportBody += "\nThe following tours have not been sold for three consecutive months:\n" +
                              $"{string.Join(", ", unsoldForThreeMonths.Select(t => t.Title))}\n" +
                              "Consider archiving these tours.";
            }

            await _emailService.SendEmailAsync(author.Email, "Monthly Sales Report", reportBody);
        }
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
