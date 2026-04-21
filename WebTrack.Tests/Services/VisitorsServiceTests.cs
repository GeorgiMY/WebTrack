using WebTrack.Core.DTOs.Visitors;

namespace WebTrack.Tests.Services;

public class VisitorsServiceTests
{
    [Test]
    public async Task GetAllUserVisitors_ReturnsVisitorsForUserWebsitesOnly()
    {
        ApplicationDbContext context = CreateContext();
        User owner = new() { Id = "owner", UserName = "owner", Email = "owner@test.com" };
        User other = new() { Id = "other", UserName = "other", Email = "other@test.com" };
        Website websiteForOwner = new() { Name = "Owner site", BaseUrl = "https://owner.test", WsSecret = "x", Users = new List<User> { owner } };
        Website websiteForOther = new() { Name = "Other site", BaseUrl = "https://other.test", WsSecret = "y", Users = new List<User> { other } };
        Visitor matchingVisitor = new() { Websites = new List<Website> { websiteForOwner } };
        Visitor nonMatchingVisitor = new() { Websites = new List<Website> { websiteForOther } };

        await context.Visitors.AddRangeAsync(matchingVisitor, nonMatchingVisitor);
        await context.SaveChangesAsync();

        VisitorsService service = new(context);

        List<VisitorListItemDto> result = await service.GetAllUserVisitors("owner");

        result.Should().HaveCount(1);
    }

    [Test]
    public async Task GetAllVisitors_ReturnsAllVisitors()
    {
        ApplicationDbContext context = CreateContext();
        await context.Visitors.AddRangeAsync(new Visitor(), new Visitor());
        await context.SaveChangesAsync();
        VisitorsService service = new(context);

        List<VisitorListItemDto> result = await service.GetAllVisitors();

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
