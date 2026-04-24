using Microsoft.EntityFrameworkCore;
using WebTrack.Core.Contracts;
using WebTrack.Core.DTOs.Visitors;
using WebTrack.Data;
using WebTrack.Data.Entities;

namespace WebTrack.Core.Services
{
    public class VisitorsService : IVisitorsService
    {
        private readonly ApplicationDbContext _context;

        public VisitorsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task EndSessionAsync(string connectionId)
        {
            Visitor? visitor = await _context.Visitors
                .Include(v => v.Sessions)
                .FirstOrDefaultAsync(v => v.ConnectionId == connectionId);

            if (visitor == null) return;

            Session? session = visitor.Sessions
                .Where(s => s.EndedAtUtc == null)
                .OrderByDescending(s => s.StartedAtUtc)
                .FirstOrDefault();

            if (session != null)
            {
                session.EndedAtUtc = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<VisitorListItemDto>> GetAllUserVisitors(string currentUserId)
        {
            List<VisitorListItemDto> allUserVisitors = await _context.Visitors
                .Where(visitor => visitor.Websites
                    .Any(website => website.Users
                        .Any(user => user.Id == currentUserId)))
                .Select(visitorListItemDto => new VisitorListItemDto
                {
                    Id = visitorListItemDto.Id,
                    ConnectionId = visitorListItemDto.ConnectionId,
                    FirstSeenAt = visitorListItemDto.FirstSeenAt,
                    UserAgent = visitorListItemDto.UserAgent,
                    Sessions = visitorListItemDto.Sessions,
                    Websites = visitorListItemDto.Websites
                })
                .ToListAsync();

            return allUserVisitors;
        }

        // For admin
        public async Task<List<VisitorListItemDto>> GetAllVisitors()
        {
            List<VisitorListItemDto> allVisitors = await _context.Visitors
                .Select(visitor => new VisitorListItemDto
                {
                    Id = visitor.Id,
                    ConnectionId = visitor.ConnectionId,
                    FirstSeenAt = visitor.FirstSeenAt,
                    UserAgent = visitor.UserAgent,
                    Websites = visitor.Websites,
                    Sessions = visitor.Sessions,
                })
                .ToListAsync();

            return allVisitors;
        }

        public async Task<bool> DeleteVisitorAsync(Guid visitorId, string? currentUserId, bool isAdmin)
        {
            var query = _context.Visitors.AsQueryable();

            if (!isAdmin)
            {
                if (string.IsNullOrWhiteSpace(currentUserId))
                {
                    return false;
                }

                query = query.Where(visitor => visitor.Websites.Any(website => website.Users.Any(user => user.Id == currentUserId)));
            }

            var visitor = await query.FirstOrDefaultAsync(v => v.Id == visitorId);
            if (visitor == null)
            {
                return false;
            }

            _context.Visitors.Remove(visitor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task LogVisitorActivityAsync(string connectionId, string websiteId, string userAgent)
        {
            Website? website = await _context.Websites.FirstOrDefaultAsync(w => w.Id.ToString() == websiteId);
            if (website == null) return;

            Visitor? visitor = await _context.Visitors
                .Include(v => v.Websites)
                .FirstOrDefaultAsync(v => v.ConnectionId == connectionId);

            if (visitor == null)
            {
                visitor = new Visitor { ConnectionId = connectionId, UserAgent = userAgent };
                await _context.Visitors.AddAsync(visitor);
            }

            if (!visitor.Websites.Any(w => w.Id == website.Id))
                visitor.Websites.Add(website);

            Session session = new Session { WebsiteId = website.Id, VisitorId = visitor.Id, Browser = "Unknown", Os = "Unknown", DeviceType = "Unknown", Referrer = "Unknown", LandingPagePath = "Unknown" };

            await _context.Sessions.AddAsync(session);
            await _context.SaveChangesAsync();
        }
    }
}
