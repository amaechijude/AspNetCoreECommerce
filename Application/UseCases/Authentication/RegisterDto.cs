using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.Application.UseCases.Authentication
{
    public class RegisterDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;
        [Required, MinLength(6)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required, MinLength(3)]
        public string FirstName {get; set;} = string.Empty;
        [Required, MinLength(3)]
        public string LastName {get; set;} = string.Empty;
        public string PhoneNumber {get; set;} = string.Empty;
    }
}