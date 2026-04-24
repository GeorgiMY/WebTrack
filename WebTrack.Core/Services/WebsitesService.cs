using Microsoft.EntityFrameworkCore;
using WebTrack.Core.Contracts;
using WebTrack.Core.DTOs.Websites;
using WebTrack.Data;
using WebTrack.Data.Entities;

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

        public async Task<WebsiteEditDto?> GetWebsiteAsync(Guid websiteId, string? currentUserId, bool isAdmin)
        {
            var query = _context.Websites.AsQueryable();

            if (!isAdmin)
            {
                if (string.IsNullOrWhiteSpace(currentUserId))
                {
                    return null;
                }

                query = query.Where(website => website.Users.Any(user => user.Id == currentUserId));
            }

            return await query
                .Where(website => website.Id == websiteId)
                .Select(website => new WebsiteEditDto
                {
                    Id = website.Id,
                    Name = website.Name,
                    BaseUrl = website.BaseUrl,
                    WsSecret = website.WsSecret
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateWebsiteAsync(WebsiteEditDto websiteEditDto, string? currentUserId, bool isAdmin)
        {
            var query = _context.Websites.AsQueryable();

            if (!isAdmin)
            {
                if (string.IsNullOrWhiteSpace(currentUserId))
                {
                    return false;
                }

                query = query.Where(website => website.Users.Any(user => user.Id == currentUserId));
            }

            Website? website = await query.FirstOrDefaultAsync(w => w.Id == websiteEditDto.Id);
            if (website == null)
            {
                return false;
            }

            bool duplicateBaseUrl = await _context.Websites
                .AnyAsync(w => w.Id != websiteEditDto.Id && w.BaseUrl == websiteEditDto.BaseUrl);
            if (duplicateBaseUrl)
            {
                return false;
            }

            website.Name = websiteEditDto.Name;
            website.BaseUrl = websiteEditDto.BaseUrl;
            website.WsSecret = websiteEditDto.WsSecret;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteWebsiteAsync(Guid websiteId, string? currentUserId, bool isAdmin)
        {
            var query = _context.Websites.AsQueryable();

            if (!isAdmin)
            {
                if (string.IsNullOrWhiteSpace(currentUserId))
                {
                    return false;
                }

                query = query.Where(website => website.Users.Any(user => user.Id == currentUserId));
            }

            Website? website = await query.FirstOrDefaultAsync(w => w.Id == websiteId);
            if (website == null)
            {
                return false;
            }

            _context.Websites.Remove(website);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
