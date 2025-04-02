using System.Threading.Channels;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.EmailService;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Repositories.Contracts;
using AspNetCoreEcommerce.ResultResponse;
using AspNetCoreEcommerce.Services.Contracts;

namespace AspNetCoreEcommerce.Services.Implementations
{
    public class OrderSevice : IOrderSevice
    {
        private readonly IOrderRepository _orderRepository;
        private readonly Channel<EmailDto> _emailChannel;
        public OrderSevice(IOrderRepository orderRepository, Channel<EmailDto> emailChannel)
        {
            _orderRepository = orderRepository;
            _emailChannel = emailChannel;
        }

        public async Task<ResultPattern> GetOrdersByCustomerIdAsync(Guid customerId)
        {
            var orders = await _orderRepository.GetCustomerOrdersAsync(customerId);
            return ResultPattern.SuccessResult(orders.Select(o => MapOrderViewDto(o)), "");
        }

        public async Task<ResultPattern> GetOrderByOrderIdAsync(Guid orderId, Guid customerId)
        {
            var order = await _orderRepository.GetOrderByOrderIdAsync(orderId, customerId);
            if (order is null)
                return ResultPattern.FailResult("Order not found");
            return ResultPattern.SuccessResult(MapOrderViewDto(order), "Order Found");
        }


        public async Task<ResultPattern> CreateOrderAsync(Guid customerId, Guid ShippingAddressId)
        {
            var (customer, cart) = await _orderRepository.GetCartByIdAsync(customerId);
            if (cart.CartTotalAmount < 1)
                return ResultPattern.FailResult("Cannot create order with empty cart");

            var shippingAddress = await _orderRepository
                .GetShippingAddressByIdAsync(customerId, ShippingAddressId);
            if (shippingAddress is null)
                return ResultPattern.FailResult("Shipping Address not found");

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
                Customer = customer,
                CustomerName = $"{customer.FirstName} {customer.LastName}",
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
            var EmailDto = new EmailDto
            {
                EmailTo = customer.CustomerEmail,
                Name = $"{customer.FirstName} {customer.LastName}",
                Subject = "Order Confirmation",
                Body = $"Your order with reference {newOrder.OrderRefrence} has been created successfully"
            };
            await _emailChannel.Writer.WriteAsync(EmailDto);
            
            return ResultPattern.SuccessResult(MapOrderViewDto(order), "Order created");
        }


        private static OrderViewDto MapOrderViewDto(Order newOrder)
        {
            return new OrderViewDto
            {
                ReceiverName = newOrder.ReceiverName,
                OrderId = newOrder.OrderId,
                CustomerId = newOrder.CustomerId,
                CustomerName = newOrder.CustomerName,
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