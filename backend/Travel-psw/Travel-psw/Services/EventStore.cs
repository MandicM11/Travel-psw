using System;
using Travel_psw.Models;
using Travel_psw.Data;
using Microsoft.EntityFrameworkCore;

namespace Travel_psw.Services
{
    public class EventStore
    {
        private readonly ApplicationDbContext _context;

        public EventStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveEventAsync(ProblemEvent problemEvent)
        {
            problemEvent.EventType = problemEvent.GetType().Name;
            _context.Add(problemEvent);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProblemEvent>> GetEventsAsync(int problemId)
        {
            return await _context.Set<ProblemEvent>()
                .Where(e => e.ProblemId == problemId)
                .OrderBy(e => e.OccurredAt)
                .ToListAsync();
        }
    }

}
