using AspNetCoreEcommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetCoreEcommerce.Infrastructure.Data.Configurations
{
    public class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.CustomerID);
            builder.HasIndex(c => c.CustomerId).IsUnique();

            // Customer Cart relationship
            builder.HasOne(c => c.Cart)
                .WithOne(c => c.Customer)
                .HasForeignKey<Cart>(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Customer Order relationship
            builder.HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Customer ShippingAddress relationship
            builder.HasMany(c => c.ShippingAddresses)
                .WithOne(sa => sa.Customer)
                .HasForeignKey(sa => sa.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Customer Feedback relationship
            builder.HasMany(c => c.Feedbacks)
                .WithOne(f => f.Customer)
                .HasForeignKey(f => f.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);



            builder.ToTable("Customers");
        }
    }
}
