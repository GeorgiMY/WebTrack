using WebTrack.Tests.TestUtilities;

namespace WebTrack.Tests.Data;

public class DatabaseSeederTests
{
    [Test]
    public async Task SeedRoles_CreatesMissingRoles()
    {
        Mock<RoleManager<IdentityRole>> roleManagerMock = IdentityMockHelpers.CreateRoleManagerMock();
        roleManagerMock.Setup(manager => manager.RoleExistsAsync("Admin")).ReturnsAsync(false);
        roleManagerMock.Setup(manager => manager.RoleExistsAsync("User")).ReturnsAsync(false);
        roleManagerMock.Setup(manager => manager.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);

        await DatabaseSeeder.SeedRoles(roleManagerMock.Object);

        roleManagerMock.Verify(manager => manager.CreateAsync(It.Is<IdentityRole>(role => role.Name == "Admin")), Times.Once);
        roleManagerMock.Verify(manager => manager.CreateAsync(It.Is<IdentityRole>(role => role.Name == "User")), Times.Once);
    }

    [Test]
    public async Task SeedAdmin_WhenExistingAdmin_DoesNotCreateUser()
    {
        Mock<UserManager<User>> userManagerMock = IdentityMockHelpers.CreateUserManagerMock();
        userManagerMock.Setup(manager => manager.FindByEmailAsync("epicadminemail@epic.email"))
            .ReturnsAsync(new User());

        await DatabaseSeeder.SeedAdmin(userManagerMock.Object);

        userManagerMock.Verify(manager => manager.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task SeedUsers_WhenCreateSucceeds_AssignsUserRole()
    {
        Mock<UserManager<User>> userManagerMock = IdentityMockHelpers.CreateUserManagerMock();
        userManagerMock.Setup(manager => manager.FindByEmailAsync("testuseremail@epic.email"))
            .ReturnsAsync((User?)null);
        userManagerMock.Setup(manager => manager.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        userManagerMock.Setup(manager => manager.AddToRoleAsync(It.IsAny<User>(), "User"))
            .ReturnsAsync(IdentityResult.Success);

        await DatabaseSeeder.SeedUsers(userManagerMock.Object);

        userManagerMock.Verify(manager => manager.AddToRoleAsync(It.IsAny<User>(), "User"), Times.Once);
    }

    [Test]
    public async Task SeedWebsites_WhenWebsiteMissing_CreatesWebsite()
    {
        ApplicationDbContext context = CreateContext();
        User testUser = new() { Id = "user-1", UserName = "testuseremail@epic.email", Email = "testuseremail@epic.email" };
        await context.Users.AddAsync(testUser);
        await context.SaveChangesAsync();
        Mock<UserManager<User>> userManagerMock = IdentityMockHelpers.CreateUserManagerMock();
        userManagerMock.Setup(manager => manager.Users).Returns(context.Users);

        await DatabaseSeeder.SeedWebsites(userManagerMock.Object, context);

        context.Websites.Should().ContainSingle(website => website.BaseUrl == "http://localhost:5173");
    }

    private static ApplicationDbContext CreateContext()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }
}
