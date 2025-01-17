using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Services
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId);
        Task<Order> GetOrderByOrderIdAsync(int orderId);
        Task UpdateOrderStatusAsync(int order);
    }
}
