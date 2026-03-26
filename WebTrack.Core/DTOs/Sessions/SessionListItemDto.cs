using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTrack.Data.Entities;

namespace WebTrack.Core.DTOs.Sessions
{
    public class SessionListItemDto
    {
        public Guid WebsiteId { get; set; }

        public Guid VisitorId { get; set; }

        public DateTime StartedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? EndedAtUtc { get; set; }

        public string? Referrer { get; set; }
        public string? LandingPagePath { get; set; }
        public string? DeviceType { get; set; }
        public string? Browser { get; set; }
        public string? Os { get; set; }
    }
}
