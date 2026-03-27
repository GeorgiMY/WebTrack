using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTrack.Core.DTOs.Visitors;

namespace WebTrack.Core.Contracts
{
    public interface IVisitorsService
    {
        Task<List<VisitorListItemDto>> GetAllUserVisitors(string currentUserId);
        Task<List<VisitorListItemDto>> GetAllVisitors();
    }
}
