
namespace AspNetCoreEcommerce.Entities
{
    public class CartItem
    {
        public Guid CartItemId { get; set; }
        public Guid CustomerId { get; set; }
        public required Customer Customer { get; set; }
        public Guid ProductId { get; set; }
        public double ProductPrice { get; set; }
        public required Product Product { get; set; }
        public Guid CartId {get; set;}
        public Cart? Cart {get; set;}
        public int Quantity { get; set; }
        public double TotalPrice => ProductPrice * Quantity;
    }
}
