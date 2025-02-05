namespace AspNetCoreEcommerce.Entities
{
    public enum OrderStatus
    {
        Initiated = 1,
        Processing = 2,
        Shipped = 3,
    }
    public class Order
    {
        public Guid OrderId { get; set; }
        public CartItem? CartItem { get; set; }
        public string OrderRefrence = $"order{Guid.CreateVersion7()}".Replace(" ", "");
        public double Discount { get; set; }
        public double TotalOrderPrice { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public int CustormerId { get; set; }
        public Customer? Customer { get; set; }
    }
}
