using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.Authentication
{
    public class LoginDto
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public required string Email {get; set;}
        [Required(ErrorMessage = "Password is required")]
        public required string Password {get; set;}
    }

    public class VendorLoginViewDto
    {
        public required Guid VendorId {get; set;}
        public required string VendorEmail {get; set;}
        public DateTimeOffset LastloginDate {get; set;}
        public required string Token {get; set;}
    }
    
    public class CustomerLoginViewDto
    {
        public Guid CustomerId {get; set;}
        public required string CustomerEmail {get; set;}
        public DateTimeOffset LastLoginDate {get; set;}
        public required string Token {get; set;}
    }
}