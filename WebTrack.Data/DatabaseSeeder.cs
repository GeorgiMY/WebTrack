using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebTrack.Data.Entities;

namespace WebTrack.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin")) await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await roleManager.RoleExistsAsync("User")) await roleManager.CreateAsync(new IdentityRole("User"));
        }

        public static async Task SeedAdmin(UserManager<User> userManager)
        {
            string adminEmail = "epicadminemail@epic.email";
            string adminPassword = "EpicAdminPassword123!";

            if (await userManager.FindByEmailAsync(adminEmail) != null) return;

            User admin = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            IdentityResult result = await userManager.CreateAsync(admin, adminPassword);

            if (result.Succeeded) await userManager.AddToRoleAsync(admin, "Admin");
        }

        public static async Task SeedUsers(UserManager<User> userManager)
        {
            string testUserEmail = "testuseremail@epic.email";
            string testUserPassword = "TestUserPassword123!";

            if (await userManager.FindByEmailAsync(testUserEmail) != null) return;

            User testUser = new User
            {
                UserName = testUserEmail,
                Email = testUserEmail,
                EmailConfirmed = true
            };

            IdentityResult result = await userManager.CreateAsync(testUser, testUserPassword);

            if (result.Succeeded) await userManager.AddToRoleAsync(testUser, "User");
        }

        public static async Task SeedWebsites(UserManager<User> userManager, ApplicationDbContext context)
        {
            string testWebsiteName = "Test Website";
            string testWebsiteBaseURL = "http://localhost:5173";
            string testWebsiteWsSecret = "TestWsSecret";

            if (await context.Websites.Select(website => website.BaseUrl == testWebsiteBaseURL).FirstOrDefaultAsync()) return;

            Website website = new Website
            {
                BaseUrl = testWebsiteBaseURL,
                Name = testWebsiteName,
                WsSecret = testWebsiteWsSecret
            };

            User? admin = await userManager.Users.Where(user => user.Email == "testuseremail@epic.email").FirstOrDefaultAsync()
                ?? throw new Exception("[DatabaseSeeder.cs] ADMIN CAN'T BE FOUND");

            website.Users.Add(admin);

            await context.Websites.AddAsync(website);
            await context.SaveChangesAsync();
        }

        public static async Task Seed(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, ApplicationDbContext context) {
            await SeedRoles(roleManager);
            await SeedAdmin(userManager);
            await SeedUsers(userManager);
            await SeedWebsites(userManager, context);
        }
    }
}
