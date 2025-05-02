using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Application.Interfaces.Services
{
    public interface IOrderSevice
    {
        Task<ResultPattern> CreateOrderAsync(Guid customerId, Guid ShippingAddress);
        Task<ResultPattern> GetOrdersByUserIdAsync(Guid customerId);
        Task<ResultPattern> GetOrderByOrderIdAsync(Guid orderId, Guid customerId);
    }
}
