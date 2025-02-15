using AspNetCoreEcommerce.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreEcommerce.Data
{
    public partial class ApplicationDbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Vendor Product relationships
            modelBuilder.Entity<Vendor>()
                .HasMany(v => v.Products)
                .WithOne(p => p.Vendor)
                .HasForeignKey(p => p.VendorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasQueryFilter(p => !p.IsDeleted);

            // Customer Cart relationship
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.CarItems)
                .WithOne(cart => cart.Customer)
                .HasForeignKey<CartItem>(cc => cc.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasIndex(cc => cc.CustomerId)
                .IsUnique();

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

            // Customer Primary key
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.CustomerEmail)
                .IsUnique();

            //Cart items
            modelBuilder.Entity<CartItem>()
                .HasKey(c => c.CartItemId);

            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(c => c.Cart)
                .HasForeignKey(c => c.CartItemId);
        }
    }
}
