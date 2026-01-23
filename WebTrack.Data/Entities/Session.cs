namespace WebTrack.Data.Entities
{
    public class Session
    {
        public Guid Id { get; set; }
        
        public Guid WebsiteId { get; set; }
        public Website Website { get; set; } = null!;
        
        public Guid VisitorId { get; set; }
        public Visitor Visitor { get; set; } = null!;

        public DateTime StartedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? EndedAtUtc { get; set; }
        
        public string? Referrer { get; set; }
        public string? LandingPagePath { get; set; }
        public string? DeviceType { get; set; }
        public string? Browser { get; set; }
        public string? Os { get; set; }
    }
}
