using System.ComponentModel.DataAnnotations;
using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Domain.Entities
{
    public class VendorVerificationCode
    {
        [Key]
        public required Guid Id { get; set; }
        public required Guid VendorId { get; set; }
        public required Vendor Vendor { get; set; }
        public string Code { get; private set; } = $"{Guid.NewGuid()}_{GlobalConstants.GenerateVerificationCode()}";
        public DateTime ExpiresOn { get; private set; } = DateTime.UtcNow.AddMinutes(20);
        public bool IsExpired => DateTime.UtcNow > ExpiresOn;
    }
}
