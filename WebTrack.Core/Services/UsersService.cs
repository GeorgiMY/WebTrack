using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebTrack.Core.Contracts;
using WebTrack.Data.Entities;

namespace WebTrack.Core.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<User> _userManager;

        public UsersService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<List<User>> GetAllVisitors()
        {
            List<User> users = await _userManager.Users.Include(u => u.Websites).ToListAsync();
            return users;
        }
    }
}
