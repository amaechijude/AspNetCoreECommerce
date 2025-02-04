using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AspNetCoreEcommerce.Entities
{
    public class Customer
    {
        public int CustomerID { get; set; }
        [EmailAddress]
        [Required]
        public string? CustomerEmail { get; set; }
        [Required]
        [PasswordPropertyText]
        public string? PasswordHash { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string Role {get; set;} = GlobalConstants.customerRole;
        public DateTime DateJoined {get; set;}
        public CartItem? CarItems { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Payment>? Payments { get; set; }
        public ICollection<ShippingAddress>? ShippingAddresses { get; set; }
    }
}
