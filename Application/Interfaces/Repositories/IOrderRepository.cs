using AspNetCoreEcommerce.Domain.Entities;

namespace AspNetCoreEcommerce.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<IEnumerable<Order>> GetUserOrdersAsync(Guid customerId);
        Task<Order?> GetOrderByOrderIdAsync(Guid orderId, Guid customerId);
        Task<(User, Cart)> GetCartByIdAsync(Guid customerId);
        Task<ShippingAddress?> GetShippingAddressByIdAsync(Guid customerId, Guid shippingAddressId);
        Task<ICollection<OrderItem>> CreateOrderItemsAsync(ICollection<OrderItem> createOrderItems);
        Task SaveChangesAsync();
    }
}