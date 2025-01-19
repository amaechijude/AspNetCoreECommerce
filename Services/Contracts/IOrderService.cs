using AspNetCoreEcommerce.DTOs;

namespace AspNetCoreEcommerce.Services.Contracts
{
    public interface IOrderSevice
    {
        Task<OrderViewDto> CreateOrderAsync(int orderId);
        Task<IEnumerable<OrderViewDto>> GetOrdersByCustomerIdAsync(int customerId);
        Task<OrderViewDto> GetOrderByOrderIdAsync(int orderId);
        Task UpdateOrderStatusAsync(int orderId);
    }
}
