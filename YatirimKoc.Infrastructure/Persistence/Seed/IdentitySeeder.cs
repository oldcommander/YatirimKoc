using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using YatirimKoc.Domain.Entities.Identity;

namespace YatirimKoc.Infrastructure.Persistence.Seed;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        // 1️⃣ Roller
        string[] roles = { "SuperAdmin", "Admin", "Consultant" };

        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new Role
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                });
            }
        }

        // 2️⃣ SuperAdmin User
        var adminEmail = "admin@yatirimkoc.com.tr";

        var superAdmin = await userManager.FindByEmailAsync(adminEmail);

        if (superAdmin == null)
        {
            superAdmin = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                FirstName = "Super",
                LastName = "Admin",
                IsActive = true
            };

            await userManager.CreateAsync(superAdmin, "Admin123!");
            await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
        }
    }
}
