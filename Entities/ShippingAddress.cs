using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.Entities
{
    public class ShippingAddress
    {
        [Key]
        public Guid ShippingAddressId { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string? Email { get; set; }
        [Required(ErrorMessage = "First name is required")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(16)]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string? AddressOne { get; set; }
        public string? SecondAddress { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string? City { get; set; }
        [Required(ErrorMessage = "State is required")]
        public string? State { get; set; }
        [Required(ErrorMessage = "Country is required")]
        public string? Country { get; set; }
        [Required(ErrorMessage = "Poastal code is required")]
        public string? PostalCode { get; set; }
        [Required(ErrorMessage = "Customerid is required")]
        public Guid CustomerId { get; set; }
        public required Customer Customer { get; set; }

    }
}
