using WebTrack.Data.Entities;

namespace WebTrack.Core.DTOs.Visitors
{
    public class VisitorListItemDto
    {
        public ICollection<Website> Websites { get; set; } = new List<Website>();
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
        public string ConnectionId { get; set; } = null!;
        public string UserAgent { get; set; } = null!;
        public DateTime FirstSeenAt { get; set; } = DateTime.UtcNow;
    }
}
