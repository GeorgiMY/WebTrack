namespace WebTrack.Data.Entities
{
    public class WebsiteVisitor
    {
        public Guid WebsiteId { get; set; }
        public Website Website { get; set; } = null!;

        public Guid VisitorId { get; set; }
        public Visitor Visitor { get; set; } = null!;
    }
}
