using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTrack.Core.DTOs.Websites;

namespace WebTrack.Core.Contracts
{
    public interface IWebsitesService
    {
        Task<List<WebsiteListItemDto>> GetAllUserWebsites(string userId);
    }
}
