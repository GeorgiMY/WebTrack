using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebTrack.Core.Contracts;
using WebTrack.Core.DTOs.Sessions;
using WebTrack.Core.DTOs.Visitors;
using WebTrack.Core.DTOs.Websites;
using WebTrack.Data.Entities;

namespace WebTrack.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IWebsitesService _websitesService;
        private readonly ISessionsService _sessionsService;
        private readonly IVisitorsService _visitorsService;

        public AdminController(UserManager<User> userManager, IWebsitesService websitesService, ISessionsService sessionsService, IVisitorsService visitorsService)
        {
            _userManager = userManager;
            _websitesService = websitesService;
            _sessionsService = sessionsService;
            _visitorsService = visitorsService;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> Websites()
        {
            List<WebsiteListItemDto> websites = await _websitesService.GetAllWebsites();
            return View(websites);
        }
        
        public async Task<IActionResult> Sessions()
        {
            List<SessionListItemDto> sessions = await _sessionsService.GetAllSessions();
            return View(sessions);
        }
        
        public async Task<IActionResult> Visitors()
        {
            List<VisitorListItemDto> visitors = await _visitorsService.GetAllVisitors();
            return View(visitors);
        }
        
        public async Task<IActionResult> Users()
        {
            List<User> users = await _userManager.Users.ToListAsync();
            return View(users);
        }
    }
}
