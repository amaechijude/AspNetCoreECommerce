using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public partial class ApplicationDbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Vendor relationships
            modelBuilder.Entity<Vendor>()
                .HasMany(v => v.Products)
                .WithOne(p => p.Vendor)
                .HasForeignKey(p => p.VendorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product Relationship
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Category)
                .WithMany(cat => cat.Products)
                .UsingEntity<Product>();

            // Customer relationship
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.CarItems)
                .WithOne(cart => cart.Customer)
                .HasForeignKey(cart => cart.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(ord => ord.CustormerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Payments)
                .WithOne(p => p.Customer)
                .HasForeignKey(p => p.CustormerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.ShippingAddresses)
                .WithOne(s => s.Customer)
                .HasForeignKey(s => s.CustormerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
