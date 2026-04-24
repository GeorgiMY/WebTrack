using WebTrack.Core.DTOs.Sessions;

namespace WebTrack.Core.Contracts
{
    public interface ISessionsService
    {
        Task<List<SessionListItemDto>> GetAllUserSessions(string currentUserId);
        Task<List<SessionListItemDto>> GetAllSessions();
        Task<bool> DeleteSessionAsync(Guid sessionId, string? currentUserId, bool isAdmin);
    }
}
