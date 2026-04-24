using System.ComponentModel.DataAnnotations;

namespace WebTrack.Core.DTOs.Visitors
{
    public class VisitorTrackingDto
    {
        [Required(ErrorMessage = "Website ID is required.")]
        public string WebsiteId { get; set; } = null!;

        [Required(ErrorMessage = "Connection ID is required.")]
        public string ConnectionId { get; set; } = null!;
    }
}
