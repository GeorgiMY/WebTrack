namespace WebTrack.ViewModels
{
    public class WebsiteListItemViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null;
        public string BaseUrl { get; set; } = null;
        public DateTime CreatedAtUtc { get; set; }
    }
}
