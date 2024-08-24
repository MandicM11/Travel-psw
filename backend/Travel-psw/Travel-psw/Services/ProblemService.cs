using System;
using Travel_psw.Models;
using Travel_psw.Data;
using Microsoft.EntityFrameworkCore;

namespace Travel_psw.Services
{
    public class ProblemService
    {
        private readonly ApplicationDbContext _context;
        private readonly EventStore _eventStore;

        public ProblemService(ApplicationDbContext context, EventStore eventStore)
        {
            _context = context;
            _eventStore = eventStore;
        }

        public async Task<Problem> ReportProblem(int touristId, int tourId, string title, string description)
        {
            var problem = new Problem
            {
                Title = title,
                Description = description,
                Status = ProblemStatus.Pending,
                TourId = tourId,
                TouristId = touristId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Problems.Add(problem);
            await _context.SaveChangesAsync();

            var problemReportedEvent = new ProblemReportedEvent
            {
                ProblemId = problem.Id,
                Title = title,
                Description = description
            };
            await _eventStore.SaveEventAsync(problemReportedEvent);

            // Pozivanje metode za obaveštavanje autora
            NotifyAuthor(problem);

            return problem;
        }

        public async Task<Problem> ResolveProblem(int problemId)
        {
            var problem = await _context.Problems.FindAsync(problemId);
            if (problem == null) return null;

            problem.Status = ProblemStatus.Resolved;
            problem.ResolvedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var problemResolvedEvent = new ProblemResolvedEvent
            {
                ProblemId = problem.Id,
                ResolvedAt = DateTime.UtcNow
            };
            await _eventStore.SaveEventAsync(problemResolvedEvent);

            // Pozivanje metode za obaveštavanje turiste
            NotifyTourist(problem);

            return problem;
        }

        public async Task<Problem> SendProblemForReview(int problemId)
        {
            var problem = await _context.Problems.FindAsync(problemId);
            if (problem == null) return null;

            problem.Status = ProblemStatus.UnderReview;
            await _context.SaveChangesAsync();

            var problemSentForReviewEvent = new ProblemSentForReviewEvent
            {
                ProblemId = problem.Id
            };
            await _eventStore.SaveEventAsync(problemSentForReviewEvent);

            return problem;
        }

        public async Task<Problem> RejectProblem(int problemId)
        {
            var problem = await _context.Problems.FindAsync(problemId);
            if (problem == null) return null;

            problem.Status = ProblemStatus.Rejected;
            await _context.SaveChangesAsync();

            var problemRejectedEvent = new ProblemRejectedEvent
            {
                ProblemId = problem.Id
            };
            await _eventStore.SaveEventAsync(problemRejectedEvent);

            return problem;
        }

        public async Task<Problem> RebuildProblemState(int problemId)
        {
            var events = await _eventStore.GetEventsAsync(problemId);

            var problem = new Problem();

            foreach (var evt in events)
            {
                switch (evt)
                {
                    case ProblemReportedEvent e:
                        problem.Id = e.ProblemId;
                        problem.Title = e.Title;
                        problem.Description = e.Description;
                        problem.Status = ProblemStatus.Pending;
                        problem.CreatedAt = e.OccurredAt;
                        break;

                    case ProblemResolvedEvent e:
                        problem.Status = ProblemStatus.Resolved;
                        problem.ResolvedAt = e.ResolvedAt;
                        break;

                    case ProblemSentForReviewEvent e:
                        problem.Status = ProblemStatus.UnderReview;
                        break;

                    case ProblemRejectedEvent e:
                        problem.Status = ProblemStatus.Rejected;
                        break;
                }
            }

            return problem;
        }

        private void NotifyAuthor(Problem problem)
        {
            // Implementacija obaveštavanja autora ture
        }

        private void NotifyTourist(Problem problem)
        {
            // Implementacija obaveštavanja turiste
        }
    }

}
