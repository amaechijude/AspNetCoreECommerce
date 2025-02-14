using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Respositories.Contracts;
using AspNetCoreEcommerce.Services.Contracts;

namespace AspNetCoreEcommerce.Services.Implementations
{
    public class OrderSevice(IOrderRepository orderRepository) : IOrderSevice
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
         public async Task<OrderViewDto> CreateOrderAsync(Guid customerId)
         {
            var cart = await  _orderRepository.GetCartItem(customerId);

            var newOrder = new Order
            {
                OrderId = Guid.CreateVersion7(),
                CartItem = cart,
                OrderRefrence = Guid.NewGuid().ToString(),
                TotalOrderPrice = cart.TotalPrice,
                DateCreated = DateTimeOffset.UtcNow,
                CustormerId = cart.CustomerId,
                Customer = cart.Customer
            };

            var createOrder = await _orderRepository.CreateOrderAsync(newOrder);
            return new OrderViewDto
            {

            };
         }
        public async Task<IEnumerable<OrderViewDto>> GetOrdersByCustomerIdAsync(Guid customerId)
        {

        }
        public async Task<OrderViewDto> GetOrderByOrderIdAsync(Guid orderId)
        {

        }
        public async Task UpdateOrderStatusAsync(Guid orderId)
        {

        }
    }
}