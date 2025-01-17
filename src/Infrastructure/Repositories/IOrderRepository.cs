using Entities;

namespace Repositories
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId);
        Task<Order> GetOrderByOrderIdAsync(int orderId);
        Task UpdateOrderStatusAsync(Order order);
    }
}