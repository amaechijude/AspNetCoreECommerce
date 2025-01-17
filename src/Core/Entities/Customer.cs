using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
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
        public CartItem? CarItems { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Payment>? Payments { get; set; }
        public ICollection<ShippingAddress>? ShippingAddresses { get; set; }
    }
}
