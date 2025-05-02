using AspNetCoreEcommerce.Application.Interfaces.Repositories;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreEcommerce.Infrastructure.Repositories
{
    public class OrderRepository(ApplicationDbContext context) : IOrderRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Order> CreateOrderAsync(Order order)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == order.UserId);
            if (cart is not null)
                _context.Carts.Remove(cart);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }
        public async Task<IEnumerable<Order>> GetUserOrdersAsync(Guid customerId)
        {
            var order = await _context.Orders
                .Where(o => o.UserId == customerId)
                .ToListAsync();

            return order.Count > 0 ? order : [];
        }
        public async Task<Order?> GetOrderByOrderIdAsync(Guid orderId, Guid customerId)
        {
            var order = await _context.Orders
                .Where(o => o.OrderId == orderId && o.UserId == customerId)
                .FirstOrDefaultAsync();

            return order is null
                ? null
                : order;
                ;
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<(User, Cart)> GetCartByIdAsync(Guid customerId)
        {
            var customer = await _context.Users.FindAsync(customerId)
                ?? throw new KeyNotFoundException("User does not exist");

            var cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == customer.Id)
                ?? throw new KeyNotFoundException("Cart is Empty");

            return (customer, cart);
        }

        public async Task<ShippingAddress?> GetShippingAddressByIdAsync(Guid customerId, Guid shippingAddressId)
        {
            var shippingAddress = await _context.ShippingAddresses
                .Where(s => s.UserId == customerId && s.ShippingAddressId == shippingAddressId)
                .FirstOrDefaultAsync();
            return shippingAddress is null
                ? null
                : shippingAddress;
        }

        public async Task<ICollection<OrderItem>> CreateOrderItemsAsync(ICollection<OrderItem> orderItems)
        {
            _context.OrderItems.AddRange(orderItems);
            await _context.SaveChangesAsync();
            return orderItems;
        }
    }
}