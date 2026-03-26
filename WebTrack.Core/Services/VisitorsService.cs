using Microsoft.EntityFrameworkCore;
using WebTrack.Core.Contracts;
using WebTrack.Core.DTOs.Visitors;
using WebTrack.Data;

namespace WebTrack.Core.Services
{
    public class VisitorsService : IVisitorsService
    {
        private readonly ApplicationDbContext _context;

        public VisitorsService(ApplicationDbContext context)
        {
            _context = context;
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
    }
}
