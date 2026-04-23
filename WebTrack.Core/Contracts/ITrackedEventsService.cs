using WebTrack.Core.DTOs.TrackedEvents;

namespace WebTrack.Core.Contracts
{
    public interface ITrackedEventsService
    {
        Task<List<TrackedEventListItemDto>> GetAllUserTrackedEvents(string currentUserId);
        Task<List<TrackedEventListItemDto>> GetAllTrackedEvents();
        Task LogEventAsync(string visitorId, string eventType);
    }
}