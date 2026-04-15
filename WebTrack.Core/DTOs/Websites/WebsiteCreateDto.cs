namespace WebTrack.Core.DTOs.Websites
{
    public class WebsiteCreateDto
    {
        public string Name { get; set; } = null;
        public string BaseUrl { get; set; } = null;
        public string WsSecret { get; set; } = null;
    }
}
