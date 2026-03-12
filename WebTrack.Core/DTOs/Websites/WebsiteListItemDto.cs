using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTrack.Core.DTOs.Websites
{
    public class WebsiteListItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null;
        public string BaseUrl { get; set; } = null;
        public DateTime CreatedAtUtc { get; set; }
    }
}
