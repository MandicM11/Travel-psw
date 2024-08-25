using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Travel_psw.Models;
using Travel_psw.Data;

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
                Description = description,
                EventType = nameof(ProblemReportedEvent)
            };
            await _eventStore.SaveEventAsync(problemReportedEvent);

            // Pozivanje metode za obaveštavanje autora ture
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
                ResolvedAt = DateTime.UtcNow,
                EventType = nameof(ProblemResolvedEvent)
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
                ProblemId = problem.Id,
                EventType = nameof(ProblemSentForReviewEvent)
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
                ProblemId = problem.Id,
                EventType = nameof(ProblemRejectedEvent)
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

        public async Task<IEnumerable<Problem>> GetAllProblemsAsync()
        {
            return await _context.Problems.ToListAsync();
        }

        public async Task<Problem> GetProblemByIdAsync(int problemId)
        {
            return await _context.Problems.FindAsync(problemId);
        }

        public async Task UpdateProblemStatusAsync(int problemId, ProblemStatus status)
        {
            var problem = await _context.Problems.FindAsync(problemId);
            if (problem == null) return;

            problem.Status = status;
            if (status == ProblemStatus.Resolved)
            {
                problem.ResolvedAt = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();

            var problemStatusChangedEvent = new ProblemStatusChangedEvent
            {
                ProblemId = problem.Id,
                NewStatus = status,
                OccurredAt = DateTime.UtcNow,
                EventType = nameof(ProblemStatusChangedEvent)
            };
            await _eventStore.SaveEventAsync(problemStatusChangedEvent);
        }

        private void NotifyAuthor(Problem problem)
        {
            // Implementacija obaveštavanja autora ture
            // Primer: slanje emaila ili poruke
        }

        private void NotifyTourist(Problem problem)
        {
            // Implementacija obaveštavanja turiste
            // Primer: slanje emaila ili poruke
        }
    }
}
