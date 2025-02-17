//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using AspNetCoreEcommerce.Data;
//using AspNetCoreEcommerce.Entities;
//using AspNetCoreEcommerce.Respositories.Contracts;
//using Microsoft.EntityFrameworkCore;

//namespace AspNetCoreEcommerce.Repositories.Implementations
//{
//    public class OrderRepository(ApplicationDbContext context) : IOrderRepository
//    {
//        private readonly ApplicationDbContext _context = context;

//        public async Task<Order> CreateOrderAsync(Order order)
//        {
//            _context.Orders.Add(order);
//            await _context.SaveChangesAsync();
//            return order;
//        }
//        public async Task<IEnumerable<Order>> GetCustomerOrdersAsync(Guid customerId)
//        {
//            var order = await _context.Orders
//                .Where(o => o.CustormerId == customerId)
//                .ToListAsync();

//            return (order.Count == 0)? [] : order;
//        }
//        public async Task<Order> GetOrderByOrderIdAsync(Guid orderId, Guid customerId)
//        {
//            var order = await _context.Orders.FindAsync(orderId)
//                ?? throw new KeyNotFoundException("Order does not exist");

//            if (order.CustormerId != customerId)
//                throw new KeyNotFoundException("Unauthorized access");

//            return order;
//        }

//        public async Task<CartItem> GetCartItem(Guid customerId)
//        {
//            var cart = await _context.CartItems.FindAsync(customerId)
//                ?? throw new KeyNotFoundException("Unauthorized access");

//            return cart;
//        }

//        public async Task SaveUpdateOrderStatusAsync()
//        {
//           await _context.SaveChangesAsync();
//        }
//    }
//}