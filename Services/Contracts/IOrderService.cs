using AspNetCoreEcommerce.DTOs;

namespace AspNetCoreEcommerce.Services.Contracts
{
    public interface IOrderSevice
    {
        Task<OrderViewDto> CreateOrderAsync(Guid customerId);
        Task<IEnumerable<OrderViewDto>> GetOrdersByCustomerIdAsync(Guid customerId);
        Task<OrderViewDto> GetOrderByOrderIdAsync(Guid orderId);
        Task UpdateOrderStatusAsync(Guid orderId);
    }
}
