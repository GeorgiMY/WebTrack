using WebTrack.Core.DTOs.Visitors;

namespace WebTrack.Core.Contracts
{
    public interface IVisitorsService
    {
        Task<List<VisitorListItemDto>> GetAllUserVisitors(string currentUserId);
        Task<List<VisitorListItemDto>> GetAllVisitors();
        Task<bool> DeleteVisitorAsync(Guid visitorId, string? currentUserId, bool isAdmin);
        Task LogVisitorActivityAsync(string connectionId, string websiteId, string userAgent);
        Task EndSessionAsync(string connectionId);
    }
}
