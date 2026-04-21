using Microsoft.Extensions.Options;

namespace WebTrack.Tests.TestUtilities;

internal static class IdentityMockHelpers
{
    public static Mock<UserManager<User>> CreateUserManagerMock()
    {
        Mock<IUserStore<User>> store = new();

        return new Mock<UserManager<User>>(
            store.Object,
            Mock.Of<IOptions<IdentityOptions>>(),
            Mock.Of<IPasswordHasher<User>>(),
            Array.Empty<IUserValidator<User>>(),
            Array.Empty<IPasswordValidator<User>>(),
            Mock.Of<ILookupNormalizer>(),
            new IdentityErrorDescriber(),
            Mock.Of<IServiceProvider>(),
            Mock.Of<ILogger<UserManager<User>>>());
    }

    public static Mock<RoleManager<IdentityRole>> CreateRoleManagerMock()
    {
        Mock<IRoleStore<IdentityRole>> store = new();

        return new Mock<RoleManager<IdentityRole>>(
            store.Object,
            Array.Empty<IRoleValidator<IdentityRole>>(),
            Mock.Of<ILookupNormalizer>(),
            new IdentityErrorDescriber(),
            Mock.Of<ILogger<RoleManager<IdentityRole>>>());
    }
}
