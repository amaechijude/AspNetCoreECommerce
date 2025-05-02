using AspNetCoreEcommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreEcommerce.Infrastructure.Data.Seeders
{
    public static class SeedDatabase
    {
        public static async Task SeedRoleAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<UserRole>>();
            string[] roleNames = ["Admin", "User", "Vendor"];

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var role = new UserRole(roleName);
                    await roleManager.CreateAsync(role);
                }
            }
        }
    }
}
