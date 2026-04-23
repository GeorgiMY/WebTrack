using Microsoft.EntityFrameworkCore;
using WebTrack.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebTrack.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Website> Websites { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<TrackedEvent> TrackedEvents { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // WEBSITE
            builder.Entity<Website>(entity =>
            {
                entity.HasKey(website => website.Id);

                entity.Property(website => website.Name)
                      .IsRequired()
                      .HasMaxLength(128);

                entity.Property(website => website.BaseUrl)
                      .IsRequired()
                      .HasMaxLength(256);

                entity.Property(website => website.CreatedAtUtc)
                      .IsRequired();

                entity.HasMany(website => website.Users)
                      .WithMany(user => user.Websites);

                entity.HasIndex(website => website.BaseUrl)
                    .IsUnique();
            });

            // VISITOR
            builder.Entity<Visitor>(entity =>
            {
                entity.HasKey(visitor => visitor.Id);

                entity.HasMany(visitor => visitor.Websites)
                      .WithMany(website => website.Visitors);
            });

            // USER (Identity user)
            builder.Entity<User>(entity =>
            {
                entity.HasMany(user => user.Websites)
                      .WithMany(website => website.Users);
            });

            builder.Entity<Session>(entity =>
            {
                entity.HasKey(session => session.Id);

                entity.Property(session => session.WebsiteId).IsRequired();
                entity.Property(session => session.VisitorId).IsRequired();

                entity.Property(session => session.StartedAtUtc).IsRequired();
                entity.Property(session => session.Referrer).HasMaxLength(512);
                entity.Property(session => session.DeviceType).HasMaxLength(64);
                entity.Property(session => session.Browser).HasMaxLength(128);
                entity.Property(session => session.Os).HasMaxLength(128);

                entity.HasOne(session => session.Website).WithMany(website => website.Sessions).HasForeignKey(s => s.WebsiteId).OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(session => session.Visitor).WithMany(visitor => visitor.Sessions).HasForeignKey(s => s.VisitorId).OnDelete(DeleteBehavior.Cascade);
            });

            // TRACKED EVENT
            builder.Entity<TrackedEvent>(entity =>
            {
                entity.HasKey(trackedEvent => trackedEvent.Id);

                entity.Property(trackedEvent => trackedEvent.SessionId)
                      .IsRequired();

                entity.Property(trackedEvent => trackedEvent.EventType)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(trackedEvent => trackedEvent.OccurredAt)
                      .IsRequired();

                entity.HasOne(trackedEvent => trackedEvent.Session)
                      .WithMany(session => session.TrackedEvents)
                      .HasForeignKey(trackedEvent => trackedEvent.SessionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
