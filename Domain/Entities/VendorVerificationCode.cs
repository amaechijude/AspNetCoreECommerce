using System.ComponentModel.DataAnnotations;
using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Domain.Entities
{
    public class VendorVerificationCode(Vendor vendor)
    {
        [Key]
        public Guid Id { get; set; } = Guid.CreateVersion7();
        public Guid VendorId { get; set; } = vendor.VendorId;
        public Vendor Vendor { get; set; } = vendor;
        public string VerificationCode { get; private set; } = $"{Guid.NewGuid()}_{GlobalConstants.GenerateVerificationCode()}";
        public DateTime ExpiresOn { get; private set; } = DateTime.UtcNow.AddMinutes(20);
        public bool IsExpired => DateTime.UtcNow > ExpiresOn;
    }
}
