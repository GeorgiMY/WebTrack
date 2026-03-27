using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebTrack.Core.Contracts;
using WebTrack.Core.DTOs.Websites;
using WebTrack.Data;
using WebTrack.Data.Entities;

namespace WebTrack.Controllers
{
    [Authorize]
    public class WebsitesController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IWebsitesService _websitesService;
        private readonly ApplicationDbContext _context;

        public WebsitesController(UserManager<User> userManager, IWebsitesService websitesService, ApplicationDbContext context)
        {
            _userManager = userManager;
            _websitesService = websitesService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<WebsiteListItemDto> websiteDtos = new List<WebsiteListItemDto>();

            if (User.IsInRole("Admin"))
            {
                websiteDtos = await _websitesService.GetAllWebsites();
            }
            else
            {
                string? currentUserId = _userManager.GetUserId(User);

                if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

                websiteDtos = await _websitesService.GetAllUserWebsites(currentUserId);

            }

            List<string> websiteIds = websiteDtos.Select(website => website.Id.ToString()).ToList();

            return View(websiteIds);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WebsiteCreateDto websiteCreateDto)
        {
            // Checks
            //----------------------------------------------------------
            if (!ModelState.IsValid) return View(websiteCreateDto);

            Website? website = _context.Websites.Where(website => website.BaseUrl == websiteCreateDto.BaseUrl).FirstOrDefault();
            if (website != null) return View(websiteCreateDto);

            User? currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();
            //----------------------------------------------------------

            website = new Website()
            {
                Name = websiteCreateDto.Name,
                BaseUrl = websiteCreateDto.BaseUrl
            };

            website.Users.Add(currentUser);

            await _context.Websites.AddAsync(website);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
