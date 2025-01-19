using Microsoft.AspNetCore.Identity;

namespace OnlineShops
{
    public static class Seed
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            await SeedRolesAsync(serviceProvider);
            await SeedAdminUserAsync(serviceProvider);
        }

        private static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[] { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(role));
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Failed to create role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }
        }

        private static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var email = "admin@admin.com";
            var password = "Admin_123";

            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser
                {
                    UserName = email,
                    Email = email
                };

                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                var roleResult = await userManager.AddToRoleAsync(user, "Admin");
                if (!roleResult.Succeeded)
                {
                    throw new Exception($"Failed to add user to role: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                }
            }
        }
    }

}
