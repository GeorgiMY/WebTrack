using System.Security.Claims;
using WebTrack.Core.DTOs.Visitors;
using WebTrack.Tests.TestUtilities;

namespace WebTrack.Tests.Controllers;

public class VisitorsControllerTests
{
    //[Test]
    //public async Task Index_AdminUser_ReturnsAllVisitorsView()
    //{
    //    Mock<IVisitorsService> serviceMock = new();
    //    serviceMock.Setup(service => service.GetAllVisitors()).ReturnsAsync(new List<VisitorListItemDto> { new() });
    //    Mock<UserManager<User>> userManagerMock = IdentityMockHelpers.CreateUserManagerMock();
    //    VisitorsController controller = new(serviceMock.Object, userManagerMock.Object)
    //    {
    //        ControllerContext = BuildControllerContext("Admin")
    //    };

    //    IActionResult result = await controller.Index();

    //    result.Should().BeOfType<ViewResult>();
    //    serviceMock.Verify(service => service.GetAllVisitors(), Times.Once);
    //}

    //[Test]
    //public async Task Index_NonAdminWithoutUserId_ReturnsUnauthorized()
    //{
    //    Mock<IVisitorsService> serviceMock = new();
    //    Mock<UserManager<User>> userManagerMock = IdentityMockHelpers.CreateUserManagerMock();
    //    userManagerMock.Setup(manager => manager.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns((string)null!);
    //    VisitorsController controller = new(serviceMock.Object, userManagerMock.Object)
    //    {
    //        ControllerContext = BuildControllerContext()
    //    };

    //    IActionResult result = await controller.Index();

    //    result.Should().BeOfType<UnauthorizedResult>();
    //}

    //[Test]
    //public async Task Index_NonAdminWithUserId_ReturnsUserVisitors()
    //{
    //    Mock<IVisitorsService> serviceMock = new();
    //    serviceMock.Setup(service => service.GetAllUserVisitors("user-1")).ReturnsAsync(new List<VisitorListItemDto> { new() });
    //    Mock<UserManager<User>> userManagerMock = IdentityMockHelpers.CreateUserManagerMock();
    //    userManagerMock.Setup(manager => manager.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("user-1");
    //    VisitorsController controller = new(serviceMock.Object, userManagerMock.Object)
    //    {
    //        ControllerContext = BuildControllerContext()
    //    };

    //    IActionResult result = await controller.Index();

    //    result.Should().BeOfType<ViewResult>();
    //    serviceMock.Verify(service => service.GetAllUserVisitors("user-1"), Times.Once);
    //}

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
