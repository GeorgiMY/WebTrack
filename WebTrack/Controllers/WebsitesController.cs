using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using WebTrack.Core.Contracts;
using WebTrack.Core.DTOs.Websites;
using WebTrack.Data;
using WebTrack.Data.Entities;
using WebTrack.ViewModels;

namespace WebTrack.Controllers
{
    [Authorize]
    public class WebsitesController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IWebsitesService _websitesService;

        public WebsitesController(UserManager<User> userManager, IWebsitesService websitesService)
        {
            _userManager = userManager;
            _websitesService = websitesService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string? currentUserId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

            List<WebsiteListItemDto> websiteDtos = await _websitesService.GetAllUserWebsites(currentUserId);

            List<WebsiteListItemViewModel> websites = [.. websiteDtos.Select(dto => new WebsiteListItemViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                BaseUrl = dto.BaseUrl,
                CreatedAtUtc = dto.CreatedAtUtc
            })];

            return View(websites);
        }
    }
}
