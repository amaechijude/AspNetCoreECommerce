﻿using System.ComponentModel.DataAnnotations;
using AspNetCoreEcommerce.Domain.Enums;

namespace AspNetCoreEcommerce.Application.UseCases.OrderUseCase
{
    public class OrderViewDto
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public Guid ShippingAddressAddressId { get; set; }
        public required string ReceiverName { get; set; }
        public required string OrderRefrence { get; set; }
        public decimal TotalOrderAmount { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public required decimal TotalAmountToBePaid { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateUpdated { get; set; }
        //public ICollection<OrderItem> OrderItems { get; set; } = [];
    }

    public class CreateOrderDto
    {
        [Required(ErrorMessage = "CartId is Required")]
        public required Guid CartId { get; set; }
        [Required(ErrorMessage = "ShippingId is Required")]
        public required Guid ShippingAddressId { get; set; }
    }
}
