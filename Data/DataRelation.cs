using AspNetCoreEcommerce.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreEcommerce.Data
{
    public partial class ApplicationDbContext
    {
        private readonly PasswordHasher<Vendor> _passwordHasher = new();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product Primary Key
            modelBuilder.Entity<Product>()
                .HasKey(p => p.ProductId);

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.ProductId)
                .IsUnique();

             // Vendor Product relationships
            modelBuilder.Entity<Vendor>()
                .HasMany(v => v.Products)
                .WithOne(p => p.Vendor)
                .HasForeignKey(p => p.VendorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Customer unique email
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.CustomerEmail)
                .IsUnique();
            
            // Customer Primary Key
            modelBuilder.Entity<Customer>()
                .HasKey(c => c.CustomerID);

            // Customer unique Primary Key
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.CustomerID)
                .IsUnique();

            // Customer Cart relationship
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Cart)
                .WithOne(cart => cart.Customer)
                .HasForeignKey<Cart>(cc => cc.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Customer Orders Relationship
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(ord => ord.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Customer Payment Relationship
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Payments)
                .WithOne(p => p.Customer)
                .HasForeignKey(p => p.CustormerId)
                .OnDelete(DeleteBehavior.SetNull);

            // Customer ShippingAddress Relationship
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Addresses)
                .WithOne(s => s.Customer)
                .HasForeignKey(s => s.CustormerId)
                .OnDelete(DeleteBehavior.Cascade);

            //Cart items
            modelBuilder.Entity<CartItem>()
                .HasKey(c => c.CartItemId);

            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(c => c.Cart)
                .HasForeignKey(c => c.CartItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // CartItem -> Product
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // FeedBack -> Customer
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Customer)
                .WithMany(c => c.Feedbacks)
                .HasForeignKey(f => f.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // FeedBack -> Products
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Product)
                .WithMany(p => p.Feedbacks)
                .HasForeignKey(f => f.ProdcutId)
                .OnDelete(DeleteBehavior.Restrict);

            


            // Seed Vendor Data
            modelBuilder.Entity<Vendor>().HasData(
                new Vendor
                {
                    VendorId = Guid.Parse("0195193a-3ef6-743e-9d22-333bbae0764b"),
                    VendorName = "Vendor One",
                    VendorEmail = "vendorone@gmail.com",
                    PasswordHash = "password",
                    VendorBanner = Path.Combine("Upload", "Vendor", "5fd9e872-0aac-445d-bd1c-d9a9dbced9e6_.jpg"),
                    VendorPhone = "098765432109",
                    Location = "Abuja",
                    DateJoined = DateTimeOffset.Parse("21-Feb-25 7:35:19 PM +00:00")
                },
                new Vendor
                {
                    VendorId = Guid.Parse("64f3720e-f189-411e-aa8d-209ece4bd40f"),
                    VendorName = "Vendor Two",
                    VendorEmail = "vendortwo@gmail.com",
                    PasswordHash = "password",
                    VendorBanner = Path.Combine("Upload", "Vendor", "2976ce82-545e-46f1-a035-507a008cc5ed_.jpg"),
                    VendorPhone = "098765432109",
                    Location = "Abuja",
                    DateJoined = DateTimeOffset.Parse("21-Feb-25 7:35:19 PM +00:00")
                }
            );

            // Seed Product data
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = Guid.Parse("01951466-ed0b-72ac-b7f1-a562259107c8"),
                    ProductName = "Product one",
                    Description = "Product One description",
                    ImageUrl = Path.Combine(),
                    Price = 1099.99,
                    StockQuantity = 7896,
                    IsAvailable = true,
                    DiscountPercentage = 0,
                    VendorId = Guid.Parse("0195193a-3ef6-743e-9d22-333bbae0764b"),
                    CreatedAt = DateTimeOffset.Parse("21-Feb-25 7:35:19 PM +00:00")
                },
                new Product
                {
                    ProductId = Guid.Parse("97951466-ed0b-72ac-b7f1-a562259107c8"),
                    ProductName = "Product two",
                    Description = "Product Two description",
                    ImageUrl = Path.Combine(),
                    Price = 2099.99,
                    StockQuantity = 921,
                    IsAvailable = true,
                    DiscountPercentage = 0,
                    VendorId =  Guid.Parse("0195193a-3ef6-743e-9d22-333bbae0764b"),
                    CreatedAt = DateTimeOffset.Parse("21-Feb-25 7:35:19 PM +00:00")
                },
                new Product
                {
                    ProductId = Guid.Parse("21951466-ed0b-72ac-b7f1-a562259107c8"),
                    ProductName = "Product Three",
                    Description = "Product three description",
                    ImageUrl = Path.Combine(),
                    Price = 3099.99,
                    StockQuantity = 325,
                    IsAvailable = true,
                    DiscountPercentage = 10,
                    VendorId = Guid.Parse("64f3720e-f189-411e-aa8d-209ece4bd40f"),
                    CreatedAt = DateTimeOffset.Parse("21-Feb-25 7:35:19 PM +00:00")
                },
                new Product
                {
                    ProductId = Guid.Parse("01951487-ed0b-72ac-b7f1-a562259107c8"),
                    ProductName = "Product four",
                    Description = "Product Four description",
                    ImageUrl = Path.Combine(),
                    Price = 4099.99,
                    StockQuantity = 475,
                    IsAvailable = true,
                    DiscountPercentage = 20,
                    VendorId = Guid.Parse("64f3720e-f189-411e-aa8d-209ece4bd40f"),
                    CreatedAt = DateTimeOffset.Parse("21-Feb-25 7:35:19 PM +00:00")
                }
            );

        }
    }
}
