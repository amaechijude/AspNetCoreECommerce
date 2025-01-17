using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObjects;
using Entities;

namespace Services
{
    public interface IOrderSevice
    {
        Task<OrderViewDto> CreateOrderAsync(int orderId);
        Task<IEnumerable<OrderViewDto>> GetOrdersByCustomerIdAsync(int customerId);
        Task<OrderViewDto> GetOrderByOrderIdAsync(int orderId);
        Task UpdateOrderStatusAsync(int orderId);
    }
}
