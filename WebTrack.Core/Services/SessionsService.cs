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
                    Id = session.Id,
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
                    Id = visitor.Id,
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

        public async Task<bool> DeleteSessionAsync(Guid sessionId, string? currentUserId, bool isAdmin)
        {
            var query = _context.Sessions.AsQueryable();

            if (!isAdmin)
            {
                if (string.IsNullOrWhiteSpace(currentUserId))
                {
                    return false;
                }

                query = query.Where(session => session.Website.Users.Any(user => user.Id == currentUserId));
            }

            var session = await query.FirstOrDefaultAsync(s => s.Id == sessionId);
            if (session == null)
            {
                return false;
            }

            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
