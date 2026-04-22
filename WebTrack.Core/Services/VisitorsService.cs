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
            Session? session = await _context.Sessions.FirstOrDefaultAsync(s => s.Id.ToString() == connectionId);
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
                    Websites = visitor.Websites,
                    Sessions = visitor.Sessions,
                })
                .ToListAsync();

            return allVisitors;
        }

        public async Task LogVisitorActivityAsync(string websiteId, Guid visitorId, string connectionId, string userAgent, string url, string screenResolution)
        {
            Website? website = await _context.Websites.FirstOrDefaultAsync(w => w.Id.ToString() == websiteId);
            
            if (website == null) return;

            // Check if this is a brand new Visitor
            Visitor? visitor = await _context.Visitors.FindAsync(visitorId);
            if (visitor == null)
            {
                visitor = new Visitor
                {
                    Id = visitorId,
                    UserAgent = userAgent
                };
                _context.Visitors.Add(visitor);
            }

            // Find active session or create a new one
            Session? session = await _context.Sessions.FirstOrDefaultAsync(s => s.Id.ToString() == connectionId);

            if (session == null)
            {
                session = new Session
                {
                    Id = Guid.NewGuid(),
                    WebsiteId = website.Id,
                    VisitorId = visitor.Id,  
                };
                _context.Sessions.Add(session);
            }
            else
            {
                // Just update the heartbeat
            }

            await _context.SaveChangesAsync();
        }
    }
}
