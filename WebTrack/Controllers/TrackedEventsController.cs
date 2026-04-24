using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebTrack.Core.Contracts;
using WebTrack.Core.DTOs.TrackedEvents;
using WebTrack.Data.Entities;

namespace WebTrack.Controllers
{
    [Authorize]
    public class TrackedEventsController : Controller
    {
        private readonly ITrackedEventsService _trackedEventsService;
        private readonly UserManager<User> _userManager;

        public TrackedEventsController(ITrackedEventsService trackedEventsService, UserManager<User> userManager)
        {
            _trackedEventsService = trackedEventsService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            List<TrackedEventListItemDto> trackedEventListItemDtos = new List<TrackedEventListItemDto>();

            if (User.IsInRole("Admin"))
            {
                trackedEventListItemDtos = await _trackedEventsService.GetAllTrackedEvents();
            }
            else
            {
                string? currentUserId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

                trackedEventListItemDtos = await _trackedEventsService.GetAllUserTrackedEvents(currentUserId);

            }

            return View(trackedEventListItemDtos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            string? currentUserId = _userManager.GetUserId(User);
            bool isAdmin = User.IsInRole("Admin");

            bool deleted = await _trackedEventsService.DeleteSessionTrackedEventsAsync(id, currentUserId, isAdmin);
            if (!deleted)
            {
                TempData["ErrorMessage"] = "Tracked events could not be deleted.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
