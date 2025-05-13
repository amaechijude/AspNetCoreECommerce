using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Application.Interfaces.Services
{
    public interface IOrderSevice
    {
        Task<ResultPattern> CreateOrderAsync(Guid customerId);
        Task<ResultPattern> GetOrdersByUserIdAsync(Guid customerId);
        Task<ResultPattern> GetOrderByOrderIdAsync(Guid customerId, Guid orderId);
    }
}
