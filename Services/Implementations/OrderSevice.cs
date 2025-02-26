using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Repositories.Contracts;
using AspNetCoreEcommerce.Services.Contracts;

namespace AspNetCoreEcommerce.Services.Implementations
{
    public class OrderSevice(IOrderRepository orderRepository) : IOrderSevice
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        
        public async Task<IEnumerable<OrderViewDto>> GetOrdersByCustomerIdAsync(Guid customerId)
        {
            var orders = await _orderRepository.GetCustomerOrdersAsync(customerId);
            return orders.Select(o => MapOrderViewDto(o));
                
            //    (o => new OrderViewDto
            //{
            //    OrderId = o.OrderId,
            //    CustomerId = o.CustomerId,
            //    CustomerName = $"{o.Customer.FirstName} {o.Customer.LastName}",
            //    ShippingAddressAddressId = o.ShippingAddressAddressId,
            //    ReceiverName = $"{o.ShippingAddress.FirstName} {o.ShippingAddress.LastName}",
            //    OrderRefrence = o.OrderRefrence,
            //    TotalOrderAmount = o.TotalOrderAmount,
            //    ShippingCost = o.ShippingCost,
            //    TotalDiscountAmount = o.TotalDiscountAmount,
            //    TotalAmountToBePaid = o.TotalAmountToBePaid,
            //    OrderStatus = o.OrderStatus,
            //    DateCreated = o.DateCreated,
            //    DateUpdated = o.DateUpdated,

            //});
        }

        public async Task<OrderViewDto> GetOrderByOrderIdAsync(Guid orderId, Guid customerId)
        {
            var order = await _orderRepository.GetOrderByOrderIdAsync(orderId, customerId);
            return MapOrderViewDto(order);
        }


        public async Task<OrderViewDto> CreateOrderAsync(Guid customerId, Guid cartId, Guid ShippingAddressId)
        {
            var cart = await _orderRepository.GetCartByIdAsync(customerId, cartId);
            var shippingAddress = await _orderRepository.GetShippingAddressByIdAsync(customerId, ShippingAddressId);
            var cartItems = cart.CartItems;
            var createOrderItems = cartItems.Select(ci => new OrderItem
            {
                OrderItemId = Guid.CreateVersion7(),
                ProductId = ci.ProductId,
                Product = ci.Product,
                Quantity = ci.Quantity,
                UnitPrice = ci.Product.Price,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
            }).ToList();

            var newOrderItems = await _orderRepository.CreateOrderItemsAsync(createOrderItems);
            var order = new Order
            {
                OrderId = Guid.CreateVersion7(),
                CustomerId = customerId,
                Customer = cart.Customer,
                ShippingAddressAddressId = shippingAddress.ShippingAddressId,
                ShippingAddress = shippingAddress,
                OrderRefrence = Guid.NewGuid().ToString(),
                TotalOrderAmount = cart.CartTotalAmount,
                ShippingCost = 0,
                TotalDiscountAmount = 0,
                OrderStatus = OrderStatusEnum.Pending,
                OrderItems = newOrderItems,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
            };

            var newOrder = await _orderRepository.CreateOrderAsync(order);
            cart.CartItems = [];
            await _orderRepository.SaveChangesAsync();
            
            return MapOrderViewDto(order);

        }


        private static OrderViewDto MapOrderViewDto(Order newOrder)
        {
            return new OrderViewDto
            {
                CustomerName = $"{newOrder.Customer.FirstName} {newOrder.Customer.LastName}",
                ReceiverName = $"{newOrder.ShippingAddress.FirstName} {newOrder.ShippingAddress.LastName}",
                OrderId = newOrder.OrderId,
                CustomerId = newOrder.CustomerId,
                ShippingAddressAddressId = newOrder.ShippingAddressAddressId,
                OrderRefrence = newOrder.OrderRefrence,
                TotalOrderAmount = newOrder.TotalOrderAmount,
                ShippingCost = newOrder.ShippingCost,
                TotalDiscountAmount = newOrder.TotalDiscountAmount,
                TotalAmountToBePaid = newOrder.TotalAmountToBePaid,
                OrderStatus = newOrder.OrderStatus,
                DateCreated = newOrder.DateCreated,
                DateUpdated = newOrder.DateUpdated,
            };
        }
    }
}