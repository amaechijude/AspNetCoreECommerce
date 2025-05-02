using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.Domain.Entities
{
    public class ShippingAddress
    {
        [Key]
        public Guid ShippingAddressId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public required string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public required User User { get; set; }
        public ICollection<Order> Orders { get; set; } = [];
        public required string ZipCode { get; set; }
        public required string PhoneNumber { get; set; } = string.Empty;
    }
}
