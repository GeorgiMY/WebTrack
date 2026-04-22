using Microsoft.EntityFrameworkCore;
using WebTrack.Core.Contracts;
using WebTrack.Core.DTOs.Sessions;
using WebTrack.Core.DTOs.TrackedEvents;
using WebTrack.Data;

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
            List<TrackedEventListItemDto> allTrackedEvents = await _context.TrackedEvents
                .Select(trackedEvent => new TrackedEventListItemDto
                {
                    Id = trackedEvent.Id,
                    ElementData = trackedEvent.ElementData,
                    EventType = trackedEvent.EventType,
                    OccurredAtUtc = trackedEvent.OccurredAt,
                    SessionId = trackedEvent.SessionId,
                    TargetUrl = trackedEvent.TargetUrl
                })
                .ToListAsync();

            return allTrackedEvents;
        }

        public async Task<List<TrackedEventListItemDto>> GetAllUserTrackedEvents(string currentUserId)
        {
            List<TrackedEventListItemDto> allTrackedEvents = await _context.TrackedEvents
                .Where(trackedEvent => trackedEvent.Session.Website.Users
                    .Any(user => user.Id == currentUserId))
                .Select(trackedEvent => new TrackedEventListItemDto
                {
                    Id = trackedEvent.Id,
                    ElementData = trackedEvent.ElementData,
                    EventType = trackedEvent.EventType,
                    OccurredAtUtc = trackedEvent.OccurredAt,
                    SessionId = trackedEvent.SessionId,
                    TargetUrl = trackedEvent.TargetUrl
                })
                .ToListAsync();

            return allTrackedEvents;
        }
    }
}
