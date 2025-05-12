using System.ComponentModel.DataAnnotations;
using AspNetCoreEcommerce.Application.UseCases.ShippingAddressUseCase;

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
    public class LoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }

    public class ForgotPasswordDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
    }

    public class ResetPasswordDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;
        [Required, MinLength(6)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;        
    }

    public class ConfirmEmailDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Token { get; set; } = string.Empty;
    }

    public class UserProfileDto
    {
        public required UserInfoDto UserInfo { get; set; }
        public required IEnumerable<OrderSummaryDto> OrderSummaries { get; set; } = [];
        public required IEnumerable<ReviewDto> Reviews { get; set; } = [];
        public required IEnumerable<ShippingAddressViewDto> ShippingAddresses { get; set; } = [];

    }

    public class UserInfoDto
    {
        public required Guid Id { get; set; }
        public required string? Email { get; set; }
        public required string FullName { get; set; }
        public required string? PhoneNumber { get; set; }
        public DateTimeOffset DateJoined { get; set; }
        public required string Membership { get; set; }
    }

    public class OrderSummaryDto
    {
        public required Guid Id { get; set; }
        public required string OrderReference { get; set; }
        public required DateTimeOffset PlacedAt { get; set; }
        public required string Status { get; set; }
        public required decimal TotalAmount { get; set; }
    }

    public class ReviewDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Rating { get; set; }   // 
        public string Comment { get; set; } = string.Empty;
        public DateTimeOffset ReviewedAt { get; set; }
    }

    public class FetchUserDto
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
        public int CartCount { get; set; } = 0;
    }

}
