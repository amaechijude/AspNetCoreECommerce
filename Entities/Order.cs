using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCoreEcommerce.Entities
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public required Customer Customer { get; set; }
        public required string CustomerName { get; set; }
        public Guid ShippingAddressAddressId { get; set; }
        public required ShippingAddress ShippingAddress { get; set; }
        public required string ReceiverName { get; set; }
        public required string OrderRefrence { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalOrderAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingCost { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalDiscountAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public required decimal TotalAmountToBePaid { get; set; }

        [EnumDataType(typeof(OrderStatusEnum), ErrorMessage = "Invalid Order Status")]
        public OrderStatusEnum OrderStatus { get; set; }
        public PaymentStatusEnum PaymentStatus {get; set;}
        public required ICollection<OrderItem> OrderItems { get; set; } = [];
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateUpdated { get; set; }
        public Payment? Payment { get; set; }

        public void UpdateOrderStatus(OrderStatusEnum statusEnum)
        {
            OrderStatus = statusEnum;
            DateUpdated = DateTime.UtcNow;
        }

    }
}
