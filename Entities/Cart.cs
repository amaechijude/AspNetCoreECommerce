namespace AspNetCoreEcommerce.Entities
{
    public class Cart
    {
        public Guid CartId {get; set;}
        public Guid CustomerId {get; set;}
        public required Customer Customer {get; set;}
        public required ICollection<CartItem> CartItems {get; set;} = [];
        public int CartCount {get; set;}
        public double CartPrice {get; set;}
    }
}