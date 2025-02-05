
namespace AspNetCoreEcommerce.Entities
{
    public class CartItem
    {
        public Guid CartId { get; set; }
        public ICollection<Product> Products { get; set; } = [];
        public double TotalPrice { get; set; }
        public int CustomerId { get; set; }
        public required Customer Customer { get; set; }

    }
}
