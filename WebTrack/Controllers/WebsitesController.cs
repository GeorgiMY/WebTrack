using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            return View(websiteDtos);
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
            if (!ModelState.IsValid)
            {
                return View(websiteCreateDto);
            }
            
            Website? website = await _context.Websites.Where(website => website.BaseUrl == websiteCreateDto.BaseUrl).FirstOrDefaultAsync();
            if (website != null)
            {
                ModelState.AddModelError(nameof(websiteCreateDto.BaseUrl), "A website with this URL already exists.");
                return View(websiteCreateDto);
            }

            User? currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();

            website = new Website()
            {
                Name = websiteCreateDto.Name,
                BaseUrl = websiteCreateDto.BaseUrl,
                WsSecret = websiteCreateDto.WsSecret
            };

            website.Users.Add(currentUser);

            await _context.Websites.AddAsync(website);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            string? currentUserId = _userManager.GetUserId(User);
            bool isAdmin = User.IsInRole("Admin");

            WebsiteEditDto? website = await _websitesService.GetWebsiteAsync(id, currentUserId, isAdmin);
            if (website == null) return NotFound();

            return View(website);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(WebsiteEditDto websiteEditDto)
        {
            if (!ModelState.IsValid) return View("Edit", websiteEditDto);

            string? currentUserId = _userManager.GetUserId(User);
            bool isAdmin = User.IsInRole("Admin");

            if (!isAdmin && string.IsNullOrWhiteSpace(currentUserId)) return Unauthorized();

            bool updated = await _websitesService.UpdateWebsiteAsync(websiteEditDto, currentUserId, isAdmin);
            if (!updated)
            {
                ModelState.AddModelError(string.Empty, "Unable to update website. Make sure the website exists and the base URL is unique.");
                return View("Edit", websiteEditDto);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            string? currentUserId = _userManager.GetUserId(User);
            bool isAdmin = User.IsInRole("Admin");

            bool deleted = await _websitesService.DeleteWebsiteAsync(id, currentUserId, isAdmin);
            if (!deleted)
            {
                TempData["ErrorMessage"] = "Website could not be deleted.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("/Websites/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            Website? website = await _context.Websites.FindAsync(id);

            if (website == null) return NotFound();

            return View(website);
        }
    }
}
