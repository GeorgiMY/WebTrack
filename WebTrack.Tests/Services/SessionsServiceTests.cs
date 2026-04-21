using WebTrack.Core.DTOs.Sessions;

namespace WebTrack.Tests.Services;

public class SessionsServiceTests
{
    [Test]
    public async Task GetAllUserSessions_ReturnsOnlyOwnedWebsiteSessions()
    {
        ApplicationDbContext context = CreateContext();
        User owner = new() { Id = "owner", UserName = "owner", Email = "owner@test.com" };
        User other = new() { Id = "other", UserName = "other", Email = "other@test.com" };
        Website ownerWebsite = new() { Name = "Owner", BaseUrl = "https://owner.test", WsSecret = "a", Users = new List<User> { owner } };
        Website otherWebsite = new() { Name = "Other", BaseUrl = "https://other.test", WsSecret = "b", Users = new List<User> { other } };
        Visitor visitor = new();

        await context.Sessions.AddRangeAsync(
            new Session { Website = ownerWebsite, Visitor = visitor, WebsiteId = ownerWebsite.Id, VisitorId = visitor.Id, Browser = "Chrome" },
            new Session { Website = otherWebsite, Visitor = visitor, WebsiteId = otherWebsite.Id, VisitorId = visitor.Id, Browser = "Firefox" });
        await context.SaveChangesAsync();

        SessionsService service = new(context);

        List<SessionListItemDto> result = await service.GetAllUserSessions("owner");

        result.Should().HaveCount(1);
        result[0].Browser.Should().Be("Chrome");
    }

    [Test]
    public async Task GetAllSessions_ReturnsAllSessions()
    {
        ApplicationDbContext context = CreateContext();
        Website website = new() { Name = "Site", BaseUrl = "https://site.test", WsSecret = "s" };
        Visitor visitor = new();
        await context.Sessions.AddRangeAsync(
            new Session { Website = website, Visitor = visitor, WebsiteId = website.Id, VisitorId = visitor.Id },
            new Session { Website = website, Visitor = visitor, WebsiteId = website.Id, VisitorId = visitor.Id });
        await context.SaveChangesAsync();

        SessionsService service = new(context);

        List<SessionListItemDto> result = await service.GetAllSessions();

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
