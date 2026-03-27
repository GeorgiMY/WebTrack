using Microsoft.EntityFrameworkCore;
using WebTrack.Core.Contracts;
using WebTrack.Core.DTOs.Sessions;
using WebTrack.Data;

namespace WebTrack.Core.Services
{
    public class SessionsService : ISessionsService
    {
        private readonly ApplicationDbContext _context;

        public SessionsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SessionListItemDto>> GetAllUserSessions(string currentUserId)
        {
            List<SessionListItemDto> allSessions = await _context.Sessions
                .Where(session => session.Website.Users
                    .Any(user => user.Id == currentUserId))
                
                .Select(session => new SessionListItemDto
                {
                    WebsiteId = session.WebsiteId,
                    VisitorId = session.VisitorId,
                    StartedAtUtc = session.StartedAtUtc,
                    EndedAtUtc = session.EndedAtUtc,
                    Referrer = session.Referrer,
                    LandingPagePath = session.LandingPagePath,
                    DeviceType = session.DeviceType,
                    Browser = session.Browser,
                    Os = session.Os,

                })
                .ToListAsync();

            return allSessions;
        }

        // For admin
        public async Task<List<SessionListItemDto>> GetAllSessions()
        {
            List<SessionListItemDto> allSessions = await _context.Sessions
                .Select(visitor => new SessionListItemDto
                {
                    WebsiteId = visitor.WebsiteId,
                    VisitorId = visitor.VisitorId,
                    StartedAtUtc = visitor.StartedAtUtc,
                    EndedAtUtc = visitor.EndedAtUtc,
                    Referrer = visitor.Referrer,
                    LandingPagePath = visitor.LandingPagePath,
                    DeviceType = visitor.DeviceType,
                    Browser = visitor.Browser,
                    Os = visitor.Os,

                })
                .ToListAsync();

            return allSessions;
        }
    }
}
