using Microsoft.EntityFrameworkCore;
using WebTrack.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebTrack.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Website> Websites { get; set; }
        public DbSet<Visitor> Visitors { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
