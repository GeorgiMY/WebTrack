using System.ComponentModel.DataAnnotations;

namespace WebTrack.Data.Entities
{
    public class Visitor
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserAgent { get; set; } = null!;
        public DateTime FirstSeenAt { get; set; } = DateTime.UtcNow;
        public ICollection<Website> Websites { get; set; } = new List<Website>();
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
