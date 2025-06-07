using AspNetCoreEcommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetCoreEcommerce.Infrastructure.Data.Configurations
{
    public class ShippingAddressConfig : IEntityTypeConfiguration<ShippingAddress>
    {
        public void Configure(EntityTypeBuilder<ShippingAddress> builder)
        {
            builder.HasKey(sa => sa.ShippingAddressId);
            builder.HasIndex(sa => sa.ShippingAddressId);
            builder.Property(sa => sa.AddressLine1).IsRequired();

            // ShippingAddress Order relationship
            // builder.HasMany(sa => sa.Orders)
            //     .WithOne(o => o.ShippingAddress)
            //     .HasForeignKey(o => o.ShippingAddressId)
            //     .OnDelete(DeleteBehavior.SetNull);

            builder.ToTable("ShippingAddresses");
        }
    }
}
