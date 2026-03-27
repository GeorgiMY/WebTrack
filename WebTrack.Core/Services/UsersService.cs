using Microsoft.AspNetCore.Identity;
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
        public List<User> GetAllVisitors()
        {
            List<User> users = _userManager.Users.ToList();
            return users;
        }
    }
}
