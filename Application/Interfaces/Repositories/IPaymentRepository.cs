using AspNetCoreEcommerce.Domain.Entities;

namespace AspNetCoreEcommerce.Application.Interfaces.Repositories
{
    public interface IPaymentRepository
    {
        Task<ShippingAddress?> GetShippingAddressByIdAsync(Guid UserId, Guid ShippingAddressId);
        Task<bool> CheckExistingPaymentAsync(Guid orderId);
        Task<Order?> GetUserOrderById(Guid UserId, Guid OrderId);
        void AddPayment(Payment payment);
        Task SaveChangesAsync();
    }
}