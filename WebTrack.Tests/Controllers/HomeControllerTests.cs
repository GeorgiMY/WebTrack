namespace WebTrack.Tests;

public class HomeControllerTests
{
    [Test]
    public void Index_ReturnsView()
    {
        Mock<ILogger<HomeController>> loggerMock = new();
        HomeController controller = new(loggerMock.Object);

        IActionResult result = controller.Index();

        result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void Error_ReturnsViewWithModel()
    {
        Mock<ILogger<HomeController>> loggerMock = new();
        HomeController controller = new(loggerMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        IActionResult result = controller.Error();

        ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.Model.Should().BeOfType<WebTrack.Models.ErrorViewModel>();
    }
}
