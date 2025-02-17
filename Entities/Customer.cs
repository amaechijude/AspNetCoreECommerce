using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AspNetCoreEcommerce.Entities
{
    public class Customer
    {
        public Guid CustomerID { get; set; }
        [EmailAddress]
        [Required]
        public required string CustomerEmail { get; set; }
        [Required]
        [PasswordPropertyText]
        public string? PasswordHash { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string Role {get; set;} = GlobalConstants.customerRole;
        public DateTimeOffset SignupDate {get; set;}
        public DateTimeOffset LastLogin { get; set; }
        public ICollection<CartItem>? CartItems { get; set; }
        public Cart? Cart { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Payment>? Payments { get; set; }
        public ICollection<ShippingAddress>? ShippingAddresses { get; set; }
    }
}
