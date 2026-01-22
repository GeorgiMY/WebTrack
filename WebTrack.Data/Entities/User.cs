using Microsoft.AspNetCore.Identity;

namespace WebTrack.Data.Entities
{
    public class User : IdentityUser
    {
        public ICollection<Website> Websites { get; set; } = new List<Website>();
    }
}
