namespace Travel_psw.Services
{
    using System.Linq;
    using System.Collections.Generic;
    using Travel_psw.Data;
    using Travel_psw.Models;

    public class RecommendationService
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public RecommendationService(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // Automatic recommendation when a new tour is created
        public void RecommendNewTour(Tour newTour)
        {
            var interestedUsers = _context.Users
                .Where(u => u.Interests.Contains(newTour.Category))
                .ToList();

            foreach (var user in interestedUsers)
            {
                string subject = "New Recommended Tour for You!";
                string body = $"Hello {user.FirstName}, a new tour {newTour.Title} that matches your interests is now available.";
                _emailService.SendEmailAsync(user.Email, subject, body);
            }
        }

        // Recommendation upon user request
        public List<Tour> GetRecommendations(User user, string selectedDifficulty)
        {
            var recommendedTours = _context.Tours
                .Where(t => user.Interests.Contains(t.Category) && t.Difficulty == selectedDifficulty)
                .ToList();

            return recommendedTours;
        }

        // Method for sending recommendations via email upon user request
        public void SendRecommendationsByEmail(User user, string selectedDifficulty)
        {
            var tours = GetRecommendations(user, selectedDifficulty);
            if (tours.Any())
            {
                string subject = "Recommended Tours for You!";
                string body = $"Hello {user.FirstName}, we recommend the following tours for you:\n";
                foreach (var tour in tours)
                {
                    body += $"{tour.Title} - Difficulty: {tour.Difficulty}\n";
                }
                _emailService.SendEmailAsync(user.Email, subject, body);
            }
            else
            {
                string subject = "No Available Recommendations";
                string body = $"Hello {user.FirstName}, currently there are no tours that match your criteria.";
                _emailService.SendEmailAsync(user.Email, subject, body);
            }
        }
    }

}
