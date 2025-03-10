using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.ResultResponse;

namespace AspNetCoreEcommerce.Services.Contracts
{
    public interface IOrderSevice
    {
        Task<ResultPattern> CreateOrderAsync(Guid customerId, Guid ShippingAddress);
        Task<ResultPattern> GetOrdersByCustomerIdAsync(Guid customerId);
        Task<ResultPattern> GetOrderByOrderIdAsync(Guid orderId, Guid customerId);
    }
}
