using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.Entities
{
    public class VerificationCode
    {
        [Key]
        [EmailAddress]
        public required string Email { get; set; }
        public required string Code { get; set; }
    }
}
