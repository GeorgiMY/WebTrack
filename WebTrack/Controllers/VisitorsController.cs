using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using WebTrack.Core.Contracts;
using WebTrack.Core.DTOs.Sessions;
using WebTrack.Core.DTOs.Visitors;
using WebTrack.Data.Entities;

namespace WebTrack.Controllers
{
    //[Authorize]
    public class VisitorsController : Controller
    {
        private readonly IVisitorsService _visitorsService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<VisitorsController> _logger;

        public VisitorsController(IVisitorsService visitorsService, UserManager<User> userManager, ILogger<VisitorsController> logger)
        {
            _visitorsService = visitorsService;
            _userManager = userManager;
            _logger = logger;
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

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost("Visitors/Create")]
        public async Task<IActionResult> Create([FromBody] VisitorTrackingDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.WebsiteId)) return BadRequest();

            // Grab the User-Agent directly from the HTTP Request headers
            string userAgent = Request.Headers.UserAgent.ToString();

            await _visitorsService.LogVisitorActivityAsync(dto.ConnectionId, dto.WebsiteId, userAgent);

            return Ok();
        }
    }
}
