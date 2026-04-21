namespace WebTrack.Tests;

public class AdminControllerTests
{
    [Test]
    public void Index_ReturnsView()
    {
        Mock<IUsersService> usersServiceMock = new();
        AdminController controller = new(usersServiceMock.Object);

        IActionResult result = controller.Index();

        result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public async Task Users_ReturnsViewWithUsers()
    {
        Mock<IUsersService> usersServiceMock = new();
        usersServiceMock.Setup(service => service.GetAllVisitors())
            .ReturnsAsync(new List<User> { new() { Id = "1", UserName = "u1", Email = "u1@test.com" } });
        AdminController controller = new(usersServiceMock.Object);

        IActionResult result = await controller.Users();

        ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.Model.Should().BeAssignableTo<List<User>>();
    }
}
