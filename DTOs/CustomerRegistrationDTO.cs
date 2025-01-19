using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.DTOs
{
    public class CustomerRegistrationDTO
    {
        [EmailAddress]
        [Required]
        public string? CustomerEmail { get; set; }
        [Required]
        [PasswordPropertyText]
        public string? Password { get; set; }
        [Required]
        public string? CustomerName { get; set; }
        [Required]
        public string? CustomerPhone { get; set; }
    }
}
