using AspNetCoreEcommerce.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreEcommerce.Data
{
    public partial class ApplicationDbContext
    {
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

            // Customer CartItems relationship
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.Customer)
                .HasForeignKey(ci => ci.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

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
                .HasMany(c => c.ShippingAddresses)
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

        }
    }
}
