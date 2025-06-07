using AspNetCoreEcommerce.Domain.Entities;

namespace AspNetCoreEcommerce.Application.Interfaces.Repositories
{
    public interface IPaymentRepository
    {
        Task<ShippingAddress?> GetShippingAddressByIdAsync(Guid UserId, Guid ShippingAddressId);
        Task<bool> CheckExistingPaymentAsync(Guid orderId);
        Task<Order?> GetUserOrderById(Guid UserId, Guid OrderId);
        Task<bool> AddPayment(Payment payment);
        Task SaveChangesAsync();
    }
}