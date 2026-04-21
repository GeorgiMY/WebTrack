using System.Security.Claims;
using WebTrack.Core.DTOs.Websites;
using WebTrack.Tests.TestUtilities;

namespace WebTrack.Tests;

public class WebsitesControllerTests
{
    [Test]
    public async Task Index_Admin_ReturnsAllWebsites()
    {
        Mock<UserManager<User>> userManagerMock = IdentityMockHelpers.CreateUserManagerMock();
        Mock<IWebsitesService> websitesServiceMock = new();
        websitesServiceMock.Setup(service => service.GetAllWebsites()).ReturnsAsync(new List<WebsiteListItemDto> { new() });
        ApplicationDbContext context = CreateContext();
        WebsitesController controller = new(userManagerMock.Object, websitesServiceMock.Object, context)
        {
            ControllerContext = BuildContext("Admin")
        };

        IActionResult result = await controller.Index();

        result.Should().BeOfType<ViewResult>();
        websitesServiceMock.Verify(service => service.GetAllWebsites(), Times.Once);
    }

    [Test]
    public async Task Index_NonAdminWithoutUserId_ReturnsUnauthorized()
    {
        Mock<UserManager<User>> userManagerMock = IdentityMockHelpers.CreateUserManagerMock();
        userManagerMock.Setup(manager => manager.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns((string)null!);
        Mock<IWebsitesService> websitesServiceMock = new();
        ApplicationDbContext context = CreateContext();
        WebsitesController controller = new(userManagerMock.Object, websitesServiceMock.Object, context)
        {
            ControllerContext = BuildContext()
        };

        IActionResult result = await controller.Index();

        result.Should().BeOfType<UnauthorizedResult>();
    }

    [Test]
    public async Task Create_Post_InvalidModelState_ReturnsBadRequest()
    {
        Mock<UserManager<User>> userManagerMock = IdentityMockHelpers.CreateUserManagerMock();
        Mock<IWebsitesService> websitesServiceMock = new();
        ApplicationDbContext context = CreateContext();
        WebsitesController controller = new(userManagerMock.Object, websitesServiceMock.Object, context);
        controller.ModelState.AddModelError("Name", "required");

        IActionResult result = await controller.Create(new WebsiteCreateDto());

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task Create_Post_DuplicateBaseUrl_ReturnsViewWithModel()
    {
        ApplicationDbContext context = CreateContext();
        await context.Websites.AddAsync(new Website { Name = "Existing", BaseUrl = "https://dupe.test", WsSecret = "x" });
        await context.SaveChangesAsync();
        Mock<UserManager<User>> userManagerMock = IdentityMockHelpers.CreateUserManagerMock();
        Mock<IWebsitesService> websitesServiceMock = new();
        WebsitesController controller = new(userManagerMock.Object, websitesServiceMock.Object, context);
        WebsiteCreateDto dto = new() { Name = "New", BaseUrl = "https://dupe.test", WsSecret = "ws" };

        IActionResult result = await controller.Create(dto);

        ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.Model.Should().Be(dto);
    }

    [Test]
    public async Task Create_Post_WhenUserMissing_ReturnsUnauthorized()
    {
        ApplicationDbContext context = CreateContext();
        Mock<UserManager<User>> userManagerMock = IdentityMockHelpers.CreateUserManagerMock();
        userManagerMock.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync((User?)null);
        Mock<IWebsitesService> websitesServiceMock = new();
        WebsitesController controller = new(userManagerMock.Object, websitesServiceMock.Object, context);

        IActionResult result = await controller.Create(new WebsiteCreateDto { Name = "A", BaseUrl = "https://a.test", WsSecret = "s" });

        result.Should().BeOfType<UnauthorizedResult>();
    }

    [Test]
    public async Task Create_Post_ValidRequest_PersistsAndRedirects()
    {
        ApplicationDbContext context = CreateContext();
        User user = new() { Id = "u1", UserName = "u1@test.com", Email = "u1@test.com" };
        Mock<UserManager<User>> userManagerMock = IdentityMockHelpers.CreateUserManagerMock();
        userManagerMock.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        Mock<IWebsitesService> websitesServiceMock = new();
        WebsitesController controller = new(userManagerMock.Object, websitesServiceMock.Object, context);

        IActionResult result = await controller.Create(new WebsiteCreateDto { Name = "Site", BaseUrl = "https://site.test", WsSecret = "secret" });

        result.Should().BeOfType<RedirectToActionResult>();
        context.Websites.Should().ContainSingle(website => website.BaseUrl == "https://site.test");
    }

    [Test]
    public async Task Details_WhenWebsiteMissing_ReturnsNotFound()
    {
        ApplicationDbContext context = CreateContext();
        Mock<UserManager<User>> userManagerMock = IdentityMockHelpers.CreateUserManagerMock();
        Mock<IWebsitesService> websitesServiceMock = new();
        WebsitesController controller = new(userManagerMock.Object, websitesServiceMock.Object, context);

        IActionResult result = await controller.Details(Guid.NewGuid());

        result.Should().BeOfType<NotFoundResult>();
    }

    private static ApplicationDbContext CreateContext()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    private static ControllerContext BuildContext(string? role = null)
    {
        List<Claim> claims = new() { new Claim(ClaimTypes.NameIdentifier, "u1") };
        if (!string.IsNullOrEmpty(role))
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"))
            }
        };
    }
}
