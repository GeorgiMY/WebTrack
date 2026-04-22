using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebTrack.Core.Contracts;
using WebTrack.Core.DTOs.TrackedEvents;
using WebTrack.Data.Entities;

namespace WebTrack.Controllers
{
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
    }
}
