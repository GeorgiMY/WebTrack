namespace WebTrack.Tests;

public class ApplicationDbContextTests
{
    [Test]
    public void DbSets_AreConfigured()
    {
        ApplicationDbContext context = CreateContext();

        context.Websites.Should().NotBeNull();
        context.Visitors.Should().NotBeNull();
        context.Sessions.Should().NotBeNull();
    }

    [Test]
    public void Model_HasExpectedSessionPropertyMaxLengths()
    {
        ApplicationDbContext context = CreateContext();

        int referrerMaxLength = context.Model.FindEntityType(typeof(Session))!
            .FindProperty(nameof(Session.Referrer))!
            .GetMaxLength()!.Value;
        int browserMaxLength = context.Model.FindEntityType(typeof(Session))!
            .FindProperty(nameof(Session.Browser))!
            .GetMaxLength()!.Value;

        referrerMaxLength.Should().Be(512);
        browserMaxLength.Should().Be(128);
    }

    private static ApplicationDbContext CreateContext()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }
}
