using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AspNetCoreEcommerce.Domain.Enums;

namespace AspNetCoreEcommerce.Domain.Entities
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public required User User { get; set; }
        public string UserName { get; set; } = string.Empty;
        public Guid ShippingAddressId { get; set; }
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
