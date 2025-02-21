using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.Entities
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }
        public Guid CustomerId {get; set;}
        public required Customer Customer {get; set;}
        public Guid AddressId {get; set;}
        public required ShippingAddress ShippingAddress {get; set;}
        public required string OrderRefrence {get; set;}
        public required Cart Cart {get; set;}
        public double TotalBaseAmount { get; set; }
        public double TotalDiscountAmount {get; set;}
        public double ShippingCost {get; set;}
        public double TotalOrderAmount {get; set;}

        [EnumDataType(typeof(OrderStatusEnum), ErrorMessage = "Invalid Order Status")]
        public OrderStatusEnum OrderStatus { get; set; }
        public required ICollection<OrderItem> OrderItems {get; set;}
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateUpdated { get; set; }
        public Payment? Payment {get; set;}

        public void UpdateOrderStatus(OrderStatusEnum statusEnum)
        {
            OrderStatus = statusEnum;
            DateUpdated = DateTimeOffset.UtcNow;
        }
    }
}
