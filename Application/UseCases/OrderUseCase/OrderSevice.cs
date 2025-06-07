using System.Threading.Channels;
using AspNetCoreEcommerce.Application.Interfaces.Repositories;
using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Domain.Enums;
using AspNetCoreEcommerce.Infrastructure.EmailInfrastructure;
using AspNetCoreEcommerce.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace AspNetCoreEcommerce.Application.UseCases.OrderUseCase
{
    public class OrderSevice(
        IOrderRepository orderRepository,
        UserManager<User> userManager,
        Channel<EmailDto> emailChannel,
        IMemoryCache memoryCache
        ) : IOrderSevice
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly Channel<EmailDto> _emailChannel = emailChannel;
        private readonly UserManager<User> _userManager = userManager;
        private readonly IMemoryCache _memoryCache = memoryCache;


        public async Task<ResultPattern> GetOrdersByUserIdAsync(Guid customerId)
        {
            var orders = await _orderRepository.GetUserOrdersAsync(customerId);
            return ResultPattern.SuccessResult(orders.Select(o => MapOrderViewDto(o)));
        }

        public async Task<ResultPattern> GetOrderByOrderIdAsync(Guid userId, Guid orderId)
        {
            string cacheKey = $"Order_{orderId}_{userId}";
            bool inCache = _memoryCache.TryGetValue(cacheKey, out Order? cachedOrder);
            if (inCache && cachedOrder is not null)
                return ResultPattern.SuccessResult(MapOrderViewDto(cachedOrder));

            var order = await _orderRepository.GetOrderByOrderIdAsync(orderId, userId);
            if (order is null)
                return ResultPattern.FailResult("Order not found");

            _memoryCache.Set(cacheKey, order, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });
            return ResultPattern.SuccessResult(MapOrderViewDto(order));
        }


        public async Task<ResultPattern> CreateOrderAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return ResultPattern.FailResult("User not found");
            var cart = await _orderRepository.GetCartByUserIdAsync(userId);
            if (cart is null || cart.CartItems.Count == 0)
                return ResultPattern.FailResult("Cannot create order with empty cart");

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
            var newOrderItems = _orderRepository.CreateOrderItemsAsync(createOrderItems);
            var order = new Order
            {
                OrderId = Guid.CreateVersion7(),
                UserId = userId,
                User = user,
                //UserName = shippingAddress.FullName,
                //ShippingAddressId = shippingAddress.ShippingAddressId,
                //ShippingAddress = shippingAddress,
                //ReceiverName = $"{shippingAddress.FirstName} {shippingAddress.LastName}",
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

            try
            {
                
            await _orderRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "Error");
                return ResultPattern.FailResult("Error creating order");
            }
            var EmailDto = new EmailDto
            {
                Name = $"{user.UserName}",
                EmailTo = $"{user.Email}",
                Subject = "Order Confirmation",
                Body = $"Your order with reference {newOrder.OrderRefrence} has been created successfully"
            };
            await _emailChannel.Writer.WriteAsync(EmailDto);
            
            return ResultPattern.SuccessResult(order.OrderId);
        }


        private static OrderViewDto MapOrderViewDto(Order newOrder)
        {
            return new OrderViewDto
            {
                ReceiverName = newOrder.ReceiverName,
                OrderId = newOrder.OrderId,
                UserId = newOrder.UserId,
                UserName = newOrder.UserName,
                ShippingAddressAddressId = newOrder.ShippingAddressId ?? Guid.Empty,
                OrderRefrence = newOrder.OrderRefrence,
                TotalOrderAmount = newOrder.TotalOrderAmount,
                ShippingCost = newOrder.ShippingCost,
                TotalDiscountAmount = newOrder.TotalDiscountAmount,
                TotalAmountToBePaid = newOrder.TotalAmountToBePaid,
                OrderStatus = newOrder.OrderStatus,
                DateCreated = newOrder.DateCreated,
                DateUpdated = newOrder.DateUpdated,
                //OrderItems = newOrder.OrderItems,
            };
        }

    }
}