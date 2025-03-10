using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.DTOs
{
    public class VerificationRequest
    {
        [EmailAddress]
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Code { get; set; }
    }
}
