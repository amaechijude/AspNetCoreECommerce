using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.Respositories.Contracts
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId);
        Task<Order> GetOrderByOrderIdAsync(int orderId);
        Task UpdateOrderStatusAsync(Order order);
    }
}