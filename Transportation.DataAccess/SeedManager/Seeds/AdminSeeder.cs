using Microsoft.AspNetCore.Identity;
using Transportation.Buisness._0.Common.Constants.Identity;
using Transportation.DataAccess.SeedManager.Settings;
using Transportation.Entities.Entities;

namespace Transportation.DataAccess.SeedManager.Seeds
{
    public static class AdminSeeder
    {
        public static async Task SeedAsync(
            UserManager<User> userManager,
            RoleManager<Role> roleManager, AdminSeedSettings settings)
        {
            await SeedRolesAsync(roleManager);

            await SeedAdminUserAsync(userManager, settings);
        }

        private static async Task SeedRolesAsync(RoleManager<Role> roleManager)
        {
            foreach (var roleName in AppRole.GetAll())
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new Role { Name = roleName });
                }
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<User> userManager, AdminSeedSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.Username) ||
                 string.IsNullOrWhiteSpace(settings.Email) ||
                 string.IsNullOrWhiteSpace(settings.Password))
                throw new InvalidOperationException("تنظیمات AdminSeed در appsettings ناقص است");

            if (await userManager.FindByNameAsync(settings.Username) != null)
                return;

            var admin = new User
            {
                UserName = settings.Username,
                FirstName = settings.Username,
                LastName = settings.Username,
                Email = settings.Email,
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(admin, settings.Password);

            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, AppRole.Admin);
        }
    }
}
