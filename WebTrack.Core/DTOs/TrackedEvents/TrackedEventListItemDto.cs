namespace WebTrack.Core.DTOs.TrackedEvents
{
    public class TrackedEventListItemDto
    {
        public Guid Id { get; set; }

        public string EventType { get; set; } = string.Empty;

        public int EventFiredTimes { get; set; }

        public DateTime OccurredAtUtc { get; set; }

        public Guid SessionId { get; set; }
    }
}
