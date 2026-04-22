namespace WebTrack.Data.Entities
{
    public class TrackedEvent
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public Session Session { get; set; }
        public string EventType { get; set; }
        public string TargetUrl { get; set; }
        public string ElementData { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    }
}
