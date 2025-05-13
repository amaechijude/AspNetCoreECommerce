using AspNetCoreEcommerce.Domain.Entities;

namespace AspNetCoreEcommerce.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<IEnumerable<Order>> GetUserOrdersAsync(Guid customerId);
        Task<Order?> GetOrderByOrderIdAsync(Guid userId, Guid orderId);
        Task<Cart?> GetCartByUserIdAsync(Guid userId);
        Task<ShippingAddress?> GetShippingAddressByIdAsync(Guid customerId, Guid shippingAddressId);
        ICollection<OrderItem> CreateOrderItemsAsync(ICollection<OrderItem> createOrderItems);
        Task SaveChangesAsync();
    
    }
}