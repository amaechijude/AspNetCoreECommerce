using AspNetCoreEcommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreEcommerce.Infrastructure.Data.Seeders
{
    public static class SeedDatabase
    {
        public static async Task SeedRoleAsync(IServiceProvider serviceProvider, ApplicationDbContext context)
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

            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            if (!await userManager.Users.AnyAsync())
            {
                var user1 = new User("user@email.com", "09876543210", DateTimeOffset.UtcNow)
                {
                    Id = Guid.Parse("64f3720e-f189-411e-aa8d-209ece4bd40f"),
                    IsVendor = true,
                };
                var result = await userManager.CreateAsync(user1, "password");
                if (!result.Succeeded) return;
                var user = await userManager.FindByEmailAsync("user@email.com");
                if (user is null) return;
                user.EmailConfirmed = true;
                await userManager.AddToRoleAsync(user, "User");
                await userManager.AddToRoleAsync(user, "Vendor");


                var vendor = new Vendor
                {
                    VendorId = Guid.Parse("01951469-ed0b-72ac-b7f1-a562259107c8"),
                    UserId = user.Id,
                    User = user,
                    VendorName = "Bespoke",
                    VendorEmail = "shop@bespoke.com",
                    Location = "123456789012",
                    IsActivated = true,
                };
                await context.Vendors.AddAsync(vendor);
                user.Vendor = vendor;
                user.VendorId = vendor.VendorId;

                // seed new products
                List<Product> products = [
                    new Product
                    {
                        ProductId = Guid.CreateVersion7(),
                        Name = "Product 1",
                        CreatedAt = DateTimeOffset.UtcNow,
                        ImageUrl = "Products/product1.jpg",
                        Price = 2033,
                        VendorId = vendor.VendorId,
                        Vendor = vendor,
                        VendorName = vendor.VendorName
                    },

                    new Product
                    {
                        ProductId = Guid.CreateVersion7(),
                        Name = "Product 2",
                        CreatedAt = DateTimeOffset.UtcNow,
                        ImageUrl = "Products/product2.jpg",
                        Price = 9500,
                        VendorId = vendor.VendorId,
                        Vendor = vendor,
                        VendorName = vendor.VendorName
                    },

                    new Product
                    {
                        ProductId = Guid.CreateVersion7(),
                        Name = "Product 3",
                        CreatedAt = DateTimeOffset.UtcNow,
                        ImageUrl = "Products/product3.jpg",
                        Price = 4590,
                        VendorId = vendor.VendorId,
                        Vendor = vendor,
                        VendorName = vendor.VendorName
                    }
                ];
                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }
        }
    }
}
