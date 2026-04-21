using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebTrack.Core.Contracts;
using WebTrack.Core.DTOs.Visitors;
using WebTrack.Data.Entities;

namespace WebTrack.Controllers
{
    [Authorize]
    public class VisitorsController : Controller
    {
        private readonly IVisitorsService _visitorsService;
        private readonly UserManager<User> _userManager;

        public VisitorsController(IVisitorsService visitorsService, UserManager<User> userManager)
        {
            _visitorsService = visitorsService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            List<VisitorListItemDto> visitorListItemDtos = new List<VisitorListItemDto>();

            if (User.IsInRole("Admin"))
            {
                visitorListItemDtos = await _visitorsService.GetAllVisitors();
            }
            else
            {
                string? currentUserId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

                visitorListItemDtos = await _visitorsService.GetAllUserVisitors(currentUserId);

            }

            return View(visitorListItemDtos);
        }
    }
}
