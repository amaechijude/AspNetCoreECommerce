using Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
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

            // Product Category Relationship
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Category)
                .WithMany(cat => cat.Products)
                .UsingEntity(join => join.ToTable("ProductCategory"));

            // Customer Cart relationship
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.CarItems)
                .WithOne(cart => cart.Customer)
                .HasForeignKey(cart => cart.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Customer Orders Relationship
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(ord => ord.CustormerId)
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
        }
    }
}
