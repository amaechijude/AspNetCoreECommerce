using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.Respositories.Contracts
{
    public interface IOrderRepository
    {
        Task<CartItem> GetCartItem(Guid customerId);
        Task<Order> CreateOrderAsync(Order order);
        Task<IEnumerable<Order>> GetCustomerOrdersAsync(Guid customerId);
        Task<Order> GetOrderByOrderIdAsync(Guid orderId, Guid customerId);
        Task SaveUpdateOrderStatusAsync();
    }
}