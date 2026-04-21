using WebTrack.Core.DTOs.Visitors;

namespace WebTrack.Core.Contracts
{
    public interface IVisitorsService
    {
        Task<List<VisitorListItemDto>> GetAllUserVisitors(string currentUserId);
        Task<List<VisitorListItemDto>> GetAllVisitors();
    }
}
