using Microsoft.EntityFrameworkCore;

namespace WebTrack.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}   
    }
}
