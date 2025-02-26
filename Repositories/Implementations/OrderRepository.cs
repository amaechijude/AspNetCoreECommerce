using AspNetCoreEcommerce.Data;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreEcommerce.Repositories.Implementations
{
    public class OrderRepository(ApplicationDbContext context) : IOrderRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Order> CreateOrderAsync(Order order)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.CustomerId == order.CustomerId);
            if (cart is not null)
                _context.Carts.Remove(cart);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }
        public async Task<IEnumerable<Order>> GetCustomerOrdersAsync(Guid customerId)
        {
            var order = await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .ToListAsync();

            return order.Count > 0 ? order : [];
        }
        public async Task<Order> GetOrderByOrderIdAsync(Guid orderId, Guid customerId)
        {
            var order = await _context.Orders.FindAsync(orderId)
                ?? throw new KeyNotFoundException("Order does not exist");

            if (order.CustomerId != customerId)
                throw new KeyNotFoundException("Unauthorized access");

            return order;
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Cart> GetCartByIdAsync(Guid customerId, Guid cartId)
        {
            var cart = await _context.Carts
                .Where(c => c.CustomerId == customerId && c.CartId == cartId)
                .FirstOrDefaultAsync();
            return cart ?? throw new KeyNotFoundException("Cart does not exist");
        }

        public async Task<ShippingAddress> GetShippingAddressByIdAsync(Guid customerId, Guid shippingAddressId)
        {
            var shippingAddress = await _context.ShippingAddresses
                .Where(s => s.CustomerId == customerId && s.ShippingAddressId == shippingAddressId)
                .FirstOrDefaultAsync();
            return shippingAddress 
                ?? throw new KeyNotFoundException("Shipping address does not exist");
        }

        public async Task<ICollection<OrderItem>> CreateOrderItemsAsync(ICollection<OrderItem> orderItems)
        {
            _context.OrderItems.AddRange(orderItems);
            await _context.SaveChangesAsync();
            return orderItems;
        }
    }
}