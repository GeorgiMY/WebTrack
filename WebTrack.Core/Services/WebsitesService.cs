using Microsoft.EntityFrameworkCore;
using WebTrack.Core.Contracts;
using WebTrack.Core.DTOs.Websites;
using WebTrack.Data;

namespace WebTrack.Core.Services
{
    public class WebsitesService : IWebsitesService
    {
        private readonly ApplicationDbContext _context;

        public WebsitesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<WebsiteListItemDto>> GetAllUserWebsites(string userId)
        {
            List<WebsiteListItemDto> userWebsites = await _context.Websites
                .Where(website => website.Users.Any(u => u.Id == userId))
                .Select(website => new WebsiteListItemDto
                {
                    Id = website.Id,
                    Name = website.Name,
                    BaseUrl = website.BaseUrl,
                    CreatedAtUtc = website.CreatedAtUtc
                })
                .ToListAsync();
            
            return userWebsites;
        }

        // For admin
        public async Task<List<WebsiteListItemDto>> GetAllWebsites()
        {
            List<WebsiteListItemDto> allWebsites = await _context.Websites
                .Select(website => new WebsiteListItemDto
                {
                    Id = website.Id,
                    Name = website.Name,
                    BaseUrl = website.BaseUrl,
                    CreatedAtUtc = website.CreatedAtUtc
                })
                .ToListAsync();

            return allWebsites;
        }
    }
}
