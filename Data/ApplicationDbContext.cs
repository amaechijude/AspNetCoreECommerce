using AspNetCoreEcommerce.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreEcommerce.Data
{
    public partial class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
    }
}
