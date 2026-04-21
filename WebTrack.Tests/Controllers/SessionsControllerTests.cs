using System.Security.Claims;
using WebTrack.Core.DTOs.Sessions;
using WebTrack.Tests.TestUtilities;

namespace WebTrack.Tests;

public class SessionsControllerTests
{
    [Test]
    public async Task Index_AdminUser_ReturnsAllSessionsView()
    {
        Mock<ISessionsService> serviceMock = new();
        serviceMock.Setup(service => service.GetAllSessions()).ReturnsAsync(new List<SessionListItemDto> { new() });
        Mock<UserManager<User>> userManagerMock = IdentityMockHelpers.CreateUserManagerMock();
        SessionsController controller = new(serviceMock.Object, userManagerMock.Object)
        {
            ControllerContext = BuildControllerContext("Admin")
        };

        IActionResult result = await controller.Index();

        ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.Model.Should().BeAssignableTo<List<SessionListItemDto>>();
        serviceMock.Verify(service => service.GetAllSessions(), Times.Once);
    }

    [Test]
    public async Task Index_NonAdminWithoutUserId_ReturnsUnauthorized()
    {
        Mock<ISessionsService> serviceMock = new();
        Mock<UserManager<User>> userManagerMock = IdentityMockHelpers.CreateUserManagerMock();
        userManagerMock.Setup(manager => manager.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns((string)null!);
        SessionsController controller = new(serviceMock.Object, userManagerMock.Object)
        {
            ControllerContext = BuildControllerContext()
        };

        IActionResult result = await controller.Index();

        result.Should().BeOfType<UnauthorizedResult>();
    }

    [Test]
    public async Task Index_NonAdminWithUserId_ReturnsUserSessions()
    {
        Mock<ISessionsService> serviceMock = new();
        serviceMock.Setup(service => service.GetAllUserSessions("user-1")).ReturnsAsync(new List<SessionListItemDto> { new() });
        Mock<UserManager<User>> userManagerMock = IdentityMockHelpers.CreateUserManagerMock();
        userManagerMock.Setup(manager => manager.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("user-1");
        SessionsController controller = new(serviceMock.Object, userManagerMock.Object)
        {
            ControllerContext = BuildControllerContext()
        };

        IActionResult result = await controller.Index();

        result.Should().BeOfType<ViewResult>();
        serviceMock.Verify(service => service.GetAllUserSessions("user-1"), Times.Once);
    }

    private static ControllerContext BuildControllerContext(string? role = null)
    {
        List<Claim> claims = new() { new Claim(ClaimTypes.NameIdentifier, "user-1") };
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
