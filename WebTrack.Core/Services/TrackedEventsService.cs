using Microsoft.EntityFrameworkCore;
using WebTrack.Core.Contracts;
using WebTrack.Core.DTOs.TrackedEvents;
using WebTrack.Data;
using WebTrack.Data.Entities;

namespace WebTrack.Core.Services
{
    public class TrackedEventsService : ITrackedEventsService
    {
        private readonly ApplicationDbContext _context;

        public TrackedEventsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TrackedEventListItemDto>> GetAllTrackedEvents()
        {
            return await _context.TrackedEvents
                .AsNoTracking()
                .Select(trackedEvent => new TrackedEventListItemDto
                {
                    Id = trackedEvent.Id,
                    EventType = trackedEvent.EventType,
                    EventFiredTimes = trackedEvent.EventFiredTimes,
                    OccurredAtUtc = trackedEvent.OccurredAt,
                    SessionId = trackedEvent.SessionId
                })
                .ToListAsync();
        }

        public async Task<List<TrackedEventListItemDto>> GetAllUserTrackedEvents(string currentUserId)
        {
            return await _context.TrackedEvents
                .AsNoTracking()
                .Where(trackedEvent => trackedEvent.Session.Website.Users
                    .Any(user => user.Id == currentUserId))
                .Select(trackedEvent => new TrackedEventListItemDto
                {
                    Id = trackedEvent.Id,
                    EventType = trackedEvent.EventType,
                    EventFiredTimes = trackedEvent.EventFiredTimes,
                    OccurredAtUtc = trackedEvent.OccurredAt,
                    SessionId = trackedEvent.SessionId
                })
                .ToListAsync();
        }

        public async Task LogEventAsync(string visitorId, string eventType)
        {
            var sessionId = await _context.Sessions
                .Where(s => s.Visitor.ConnectionId == visitorId && s.EndedAtUtc == null)
                .OrderByDescending(s => s.StartedAtUtc)
                .Select(s => s.Id)
                .FirstOrDefaultAsync();

            if (sessionId == Guid.Empty) return;

            string normalizedType = eventType.ToUpper();

            int rowsUpdated = await _context.TrackedEvents
                .Where(e => e.SessionId == sessionId && e.EventType == normalizedType)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(e => e.EventFiredTimes, e => e.EventFiredTimes + 1)
                    .SetProperty(e => e.OccurredAt, DateTime.UtcNow));

            if (rowsUpdated == 0)
            {
                _context.TrackedEvents.Add(new TrackedEvent
                {
                    Id = Guid.NewGuid(),
                    SessionId = sessionId,
                    EventType = normalizedType,
                    EventFiredTimes = 1,
                    OccurredAt = DateTime.UtcNow
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}