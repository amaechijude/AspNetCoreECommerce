using Entities;
using System.ComponentModel.DataAnnotations;

namespace DataTransferObjects
{
    public class CustomerDTO
    {
        [EmailAddress]
        public string? CustomerEmail { get; set; }
        [Required]
        public string? CustomerName { get; set; }
        [Required]
        public string? CustomerPhone { get; set; }
        public ICollection<CartItem>? CarItems { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Payment>? Payments { get; set; }
        public ICollection<ShippingAddress>? ShippingAddresses { get; set; }
    }
}
