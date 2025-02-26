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
        }

        public async Task<OrderViewDto> GetOrderByOrderIdAsync(Guid orderId, Guid customerId)
        {
            var order = await _orderRepository.GetOrderByOrderIdAsync(orderId, customerId);
            return MapOrderViewDto(order);
        }


        public async Task<OrderViewDto> CreateOrderAsync(Guid customerId, Guid cartId, Guid ShippingAddressId)
        {
            var cart = await _orderRepository.GetCartByIdAsync(customerId, cartId);
            if (cart.CartTotalAmount <= 1)
                throw new EmptyCartException("Cannot create order with empty cart");

            var shippingAddress = await _orderRepository.GetShippingAddressByIdAsync(customerId, ShippingAddressId);
            
            var createOrderItems = cart.CartItems.Select(ci => new OrderItem
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
                ReceiverName = $"{shippingAddress.FirstName} {shippingAddress.LastName}",
                OrderRefrence = Guid.NewGuid().ToString(),
                TotalOrderAmount = cart.CartTotalAmount,
                ShippingCost = 0,
                TotalDiscountAmount = 0,
                TotalAmountToBePaid = cart.CartTotalAmount + 0 - 0, // + shipAddressv- discount
                OrderStatus = OrderStatusEnum.Pending,
                OrderItems = newOrderItems,
                DateCreated = DateTimeOffset.UtcNow,
                DateUpdated = DateTimeOffset.UtcNow,
            };

            var newOrder = await _orderRepository.CreateOrderAsync(order);
            await _orderRepository.SaveChangesAsync();
            
            return MapOrderViewDto(order);

        }


        private static OrderViewDto MapOrderViewDto(Order newOrder)
        {
            return new OrderViewDto
            {
                ReceiverName = newOrder.ReceiverName,
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

        public class EmptyCartException(string Message) : Exception(Message);
    }
}