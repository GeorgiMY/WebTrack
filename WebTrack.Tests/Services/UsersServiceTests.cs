using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebTrack.Tests;

public class UsersServiceTests
{
    [Test]
    public async Task GetAllVisitors_ReturnsUsersFromUserManager()
    {
        ApplicationDbContext context = CreateContext();
        UserStore<User> userStore = new(context);
        UserManager<User> userManager = new(
            userStore,
            Mock.Of<IOptions<IdentityOptions>>(),
            new PasswordHasher<User>(),
            Array.Empty<IUserValidator<User>>(),
            Array.Empty<IPasswordValidator<User>>(),
            Mock.Of<ILookupNormalizer>(),
            new IdentityErrorDescriber(),
            Mock.Of<IServiceProvider>(),
            Mock.Of<ILogger<UserManager<User>>>());

        await userManager.CreateAsync(new User { UserName = "u1@test.com", Email = "u1@test.com" }, "Pass123!");
        await userManager.CreateAsync(new User { UserName = "u2@test.com", Email = "u2@test.com" }, "Pass123!");

        UsersService service = new(userManager);

        List<User> result = await service.GetAllVisitors();

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
