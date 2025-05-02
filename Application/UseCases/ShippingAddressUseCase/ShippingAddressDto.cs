using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.Application.UseCases.ShippingAddressUseCase
{
    public class ShippingAddressDto
    {
        [Required(ErrorMessage = "First name is required")]
        public required string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public required string LastName { get; set; }
        [Required(ErrorMessage = "Phone number is required"), Phone]
        public required string Phone { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public required string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; } = string.Empty;
        [Required(ErrorMessage = "City is required")]
        public required string City { get; set; }
        [Required(ErrorMessage = "State is required")]
        public required string State { get; set; }
        [Required(ErrorMessage = "Country is required")]
        public required string Country { get; set; }
        [Required(ErrorMessage = "Poastal code is required")]
        public required string PostalCode { get; set; }
    }

    public class ShippingAddressViewDto
    {
        public Guid ShippingAddressId { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? ShippingAddressName { get; set; }
        public string? ShippingAddressPhone { get; set; }
        public string? AddressOne { get; set; }
        public string? SecondAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
    }
}
