namespace WebTrack.Core.DTOs.Websites
{
    public class WebsiteEditDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string BaseUrl { get; set; } = null!;
        public string WsSecret { get; set; } = null!;
    }
}
