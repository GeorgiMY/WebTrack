namespace WebTrack.Data.Entities
{
    public class UserWebsite
    {
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;

        public Guid WebsiteId { get; set; }
        public Website Website { get; set; } = null!;
    }
}
