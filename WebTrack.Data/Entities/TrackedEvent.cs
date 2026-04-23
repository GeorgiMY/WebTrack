namespace WebTrack.Data.Entities
{
    public class TrackedEvent
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public Session Session { get; set; }
        public string EventType { get; set; }
        public int EventFiredTimes { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    }
}
