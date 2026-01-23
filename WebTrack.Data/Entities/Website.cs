namespace WebTrack.Data.Entities
{
    public class Website
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string BaseUrl { get; set; } = null!;
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
        public ICollection<Visitor> Visitors { get; set; } = new List<Visitor>();
    }
}
