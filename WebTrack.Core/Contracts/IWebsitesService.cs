using WebTrack.Core.DTOs.Websites;

namespace WebTrack.Core.Contracts
{
    public interface IWebsitesService
    {
        Task<List<WebsiteListItemDto>> GetAllUserWebsites(string userId);
        Task<List<WebsiteListItemDto>> GetAllWebsites();
    }
}
