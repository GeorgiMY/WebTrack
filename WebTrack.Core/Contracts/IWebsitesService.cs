using WebTrack.Core.DTOs.Websites;

namespace WebTrack.Core.Contracts
{
    public interface IWebsitesService
    {
        Task<List<WebsiteListItemDto>> GetAllUserWebsites(string userId);
        Task<List<WebsiteListItemDto>> GetAllWebsites();
        Task<WebsiteEditDto?> GetWebsiteAsync(Guid websiteId, string? currentUserId, bool isAdmin);
        Task<bool> UpdateWebsiteAsync(WebsiteEditDto websiteEditDto, string? currentUserId, bool isAdmin);
        Task<bool> DeleteWebsiteAsync(Guid websiteId, string? currentUserId, bool isAdmin);
    }
}
