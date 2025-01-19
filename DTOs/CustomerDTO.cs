using AspNetCoreEcommerce.Entities;
namespace AspNetCoreEcommerce.DTOs
{
    public class CustomerDTO
    {
        public int CustomerId {get; set;}
        public string? CustomerEmail { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public ICollection<CartItem>? CarItems { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Payment>? Payments { get; set; }
        public ICollection<ShippingAddress>? ShippingAddresses { get; set; }
    }
}
