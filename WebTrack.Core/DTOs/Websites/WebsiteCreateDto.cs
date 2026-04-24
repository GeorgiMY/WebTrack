using System.ComponentModel.DataAnnotations;

namespace WebTrack.Core.DTOs.Websites
{
    public class WebsiteCreateDto
    {
        [Required(ErrorMessage = "Website name is required.")]
        [StringLength(128, ErrorMessage = "Website name must be {2} to {1} characters.", MinimumLength = 3)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Base URL is required.")]
        [Url(ErrorMessage = "Please enter a valid URL starting with http:// or https://.")]
        [StringLength(256, ErrorMessage = "Base URL cannot be longer than {1} characters.")]
        public string BaseUrl { get; set; } = null!;

        [Required(ErrorMessage = "WebSocket secret is required.")]
        [StringLength(128, ErrorMessage = "WebSocket secret must be {2} to {1} characters.", MinimumLength = 8)]
        public string WsSecret { get; set; } = null!;
    }
}
