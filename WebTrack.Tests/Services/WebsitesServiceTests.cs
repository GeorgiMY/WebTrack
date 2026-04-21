using WebTrack.Core.DTOs.Websites;

namespace WebTrack.Tests;

public class WebsitesServiceTests
{
    [Test]
    public async Task GetAllUserWebsites_ReturnsOnlyWebsitesOwnedByUser()
    {
        ApplicationDbContext context = CreateContext();
        User user1 = new() { Id = "user-1", UserName = "u1", Email = "u1@test.com" };
        User user2 = new() { Id = "user-2", UserName = "u2", Email = "u2@test.com" };
        Website first = new() { Name = "First", BaseUrl = "https://first.test", WsSecret = "s1", Users = new List<User> { user1 } };
        Website second = new() { Name = "Second", BaseUrl = "https://second.test", WsSecret = "s2", Users = new List<User> { user2 } };

        await context.Websites.AddRangeAsync(first, second);
        await context.SaveChangesAsync();

        WebsitesService service = new(context);

        List<WebsiteListItemDto> result = await service.GetAllUserWebsites("user-1");

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("First");
        result[0].BaseUrl.Should().Be("https://first.test");
    }

    [Test]
    public async Task GetAllWebsites_ReturnsAllWebsites()
    {
        ApplicationDbContext context = CreateContext();
        await context.Websites.AddRangeAsync(
            new Website { Name = "A", BaseUrl = "https://a.test", WsSecret = "sa" },
            new Website { Name = "B", BaseUrl = "https://b.test", WsSecret = "sb" });
        await context.SaveChangesAsync();

        WebsitesService service = new(context);

        List<WebsiteListItemDto> result = await service.GetAllWebsites();

        result.Should().HaveCount(2);
    }

    private static ApplicationDbContext CreateContext()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }
}
