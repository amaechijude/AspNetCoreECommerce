using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.DTOs
{
    public class VerificationRequest
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is Missing")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Verification Code is Missing")]
        public required string Code { get; set; }
    }
}
