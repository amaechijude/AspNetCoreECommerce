
namespace AspNetCoreEcommerce.Entities
{
    public class CartItem
    {
        public Guid CartId { get; set; }
        public ICollection<Product> Products { get; set; } = [];
        public double TotalPrice { get; set; }
        public Guid CustomerId { get; set; }
        public Customer? Customer {get; set;}

    }
}
