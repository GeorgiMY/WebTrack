using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebTrack.Core.Contracts;
using WebTrack.Core.DTOs.Sessions;
using WebTrack.Data.Entities;

namespace WebTrack.Controllers
{
    [Authorize]
    public class SessionsController : Controller
    {
        private readonly ISessionsService _sessionsService;
        private readonly UserManager<User> _userManager;

        public SessionsController(ISessionsService sessionsService, UserManager<User> userManager)
        {
            _sessionsService = sessionsService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            List<SessionListItemDto> sessionListItemDtos = new List<SessionListItemDto>();

            if (User.IsInRole("Admin"))
            {
                sessionListItemDtos = await _sessionsService.GetAllSessions();
            }
            else
            {
                string? currentUserId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

                sessionListItemDtos = await _sessionsService.GetAllUserSessions(currentUserId);
            }

            return View(sessionListItemDtos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            string? currentUserId = _userManager.GetUserId(User);
            bool isAdmin = User.IsInRole("Admin");

            bool deleted = await _sessionsService.DeleteSessionAsync(id, currentUserId, isAdmin);
            if (!deleted)
            {
                TempData["ErrorMessage"] = "Session could not be deleted.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
