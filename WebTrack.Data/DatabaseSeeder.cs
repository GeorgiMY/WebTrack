using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTrack.Data.Entities;

namespace WebTrack.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAdmin(UserManager<User> userManager)
        {
            string adminEmail = "epicadminemail@epic.email";
            string adminPassword = "EpicAdminPassword123!";

            if (await userManager.FindByEmailAsync(adminEmail) == null) return;

            User admin = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            IdentityResult result = await userManager.CreateAsync(admin, adminPassword);

            if (result.Succeeded) await userManager.AddToRoleAsync(admin, "admin");
        }

        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin")) await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await roleManager.RoleExistsAsync("User")) await roleManager.CreateAsync(new IdentityRole("User"));
        }
        public static async Task Seed(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager) {
            await SeedRoles(roleManager);
            await SeedAdmin(userManager);
        }
    }
}
