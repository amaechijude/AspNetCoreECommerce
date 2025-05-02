using AspNetCoreEcommerce.Domain.Entities;

namespace AspNetCoreEcommerce.Application.Interfaces.Repositories
{
    public interface IPaymentRepository
    {
        Task<User?> GetUserByIdAsync(Guid UserId);
        Task<Order?> GetUserOrderById(Guid UserId, Guid OrderId);
        Task AddPaymentAsync(Payment payment);
        Task SaveChangesAsync();
    }
}