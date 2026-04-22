namespace WebTrack.Data.Entities
{
    public class Session
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid WebsiteId { get; set; }
        public Website Website { get; set; } = null!;
        
        public Guid VisitorId { get; set; }
        public Visitor Visitor { get; set; } = null!;

        public ICollection<TrackedEvent> TrackedEvents { get; set; } = new List<TrackedEvent>();

        public DateTime StartedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? EndedAtUtc { get; set; }

        public string Referrer { get; set; } = null!;
        public string LandingPagePath { get; set; } = null!;
        public string DeviceType { get; set; } = null!;
        public string Browser { get; set; } = null!;
        public string Os { get; set; } = null!;
    }
}
