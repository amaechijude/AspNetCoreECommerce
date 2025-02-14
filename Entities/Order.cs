namespace AspNetCoreEcommerce.Entities
{
    public enum OrderStatusEnum
    {
        Initiated = 1,
        Processing = 2,
        Shipped = 3,
    }
    public class Order
    {
        public Guid OrderId { get; set; }
        public CartItem? CartItem { get; set; }
        public required string OrderRefrence {get; set;}
        public double TotalOrderPrice { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateUpdated { get; set; }
        public Guid CustormerId { get; set; }
        public required Customer Customer { get; set; }

        public void UpdateOrderStatus(OrderStatusEnum statusEnum)
        {
            OrderStatus = statusEnum;
            DateUpdated = DateTimeOffset.UtcNow;
        }
    }
}
