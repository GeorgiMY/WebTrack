using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebTrack.Core.Contracts;
using WebTrack.Core.Services;
using WebTrack.Data.Entities;

namespace WebTrack.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUsersService _usersService;

        public AdminController(IUsersService usersService)
        {
            _usersService = usersService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Users()
        {
            List<User> users = _usersService.GetAllVisitors();
            return View(users);
        }
    }
}
