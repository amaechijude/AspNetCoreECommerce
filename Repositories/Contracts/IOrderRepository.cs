using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.Repositories.Contracts
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<IEnumerable<Order>> GetCustomerOrdersAsync(Guid customerId);
        Task<Order> GetOrderByOrderIdAsync(Guid orderId, Guid customerId);
        Task SaveChangesAsync();
        Task<(Customer, Cart)> GetCartByIdAsync(Guid customerId);
        Task<ShippingAddress> GetShippingAddressByIdAsync(Guid customerId, Guid shippingAddressId);
        Task<ICollection<OrderItem>> CreateOrderItemsAsync(ICollection<OrderItem> createOrderItems);
    }
}