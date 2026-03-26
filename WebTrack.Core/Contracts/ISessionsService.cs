using WebTrack.Core.DTOs.Sessions;

namespace WebTrack.Core.Contracts
{
    public interface ISessionsService
    {
        Task<List<SessionListItemDto>> GetAllSessions();
    }
}
