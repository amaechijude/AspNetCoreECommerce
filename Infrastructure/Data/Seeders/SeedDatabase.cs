using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Shared;
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

                var random = new Random();
                var products = new List<Product>();
                var now = DateTimeOffset.UtcNow;

                // seed new products
                for (int i = 1; i <= 100; i++)
                {
                    products.Add(new Product
                    {
                        ProductId = Guid.CreateVersion7(),
                        Name = $"Product {i}",
                        Description = $"This is a detailed description for Product {i}. It offers great quality and value. Perfect for your needs.",
                        ImageUrl = $"{GlobalConstants.productSubPath}/product{random.Next(1, 11)}.jpg", 
                        Price = Math.Round(new decimal(random.Next(1000, 500000)) / 100m, 2), // Price between 10.00 and 5000.00
                        StockQuantity = random.Next(10, 200), // Stock between 10 and 199
                        DiscountPercentage = Math.Round(new decimal(random.Next(0, 3001)) / 100m, 2), // Discount 0.00% to 30.00%
                        CreatedAt = now,
                        UpdatedAt = now, // Using current name from Product.cs
                        VendorId = vendor.VendorId,
                        Vendor = vendor,
                        VendorName = vendor.VendorName,
                        Rating = random.Next(1, 6), // Rating 1 to 5
                        ReviewCount = random.Next(0, 251) // Review count 0 to 250, using current name from Product.cs
                    });
                }

                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }
        }
    }
}
