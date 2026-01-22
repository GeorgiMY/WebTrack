namespace WebTrack.Data.Entities
{
    public class Visitor
    {
        public Guid Id { get; set; }

        public ICollection<Website> Websites { get; set; } = new List<Website>();
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
