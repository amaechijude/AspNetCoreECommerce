using AspNetCoreEcommerce.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.DTOs
{
    public class OrderViewDto
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public required string CustomerName { get; set; }
        public Guid ShippingAddressAddressId { get; set; }
        public required string ReceiverName { get; set; }
        public required string OrderRefrence { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalOrderAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingCost { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalDiscountAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmountToBePaid { get; set; }

        [EnumDataType(typeof(OrderStatusEnum), ErrorMessage = "Invalid Order Status")]
        public OrderStatusEnum OrderStatus { get; set; }
        //public required ICollection<OrderItem> OrderItems { get; set; } = [];
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateUpdated { get; set; }
    }

    public class CreateOrderDto
    {
        [Required(ErrorMessage = "CartId is Required")]
        public required Guid CartId { get; set; }
        [Required(ErrorMessage = "ShippingId is Required")]
        public required Guid ShippingAddressId { get; set; }
    }
}
