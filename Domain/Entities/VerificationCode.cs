using System.ComponentModel.DataAnnotations;
using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Domain.Entities
{
    public class VerificationCode
    {
        [Key]
        public Guid Id { get; set; }
        public required Guid VendorId { get; set; }
        public required Vendor Vendor { get; set; }
        public required string Code { get; set; }
        public required string Email { get; set; }
        public int MinutesToExpire { get; set; } = 20;
        public required DateTime ExpiresIn { get; set; }
        public bool IsExpired => ExpiresIn < DateTime.UtcNow;

    }
}
