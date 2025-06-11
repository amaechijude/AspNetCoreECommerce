using AspNetCoreEcommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetCoreEcommerce.Infrastructure.Data.Configurations
{
    public class VerificationCodeConfig : IEntityTypeConfiguration<VendorVerificationCode>
    {
        public void Configure(EntityTypeBuilder<VendorVerificationCode> builder)
        {
            builder.HasKey(vc => vc.Id);
            builder.HasIndex(vc => vc.VendorId);

            builder.HasOne(vc => vc.Vendor)
                .WithOne(vd => vd.VendorVerificationCode);

            builder.ToTable("vcodes");
        }
    }
}
