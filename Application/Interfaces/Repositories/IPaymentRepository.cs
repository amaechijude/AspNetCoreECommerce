using AspNetCoreEcommerce.Domain.Entities;

namespace AspNetCoreEcommerce.Application.Interfaces.Repositories
{
    public interface IPaymentRepository
    {
        Task<Customer?> GetCustomerByIdAsync(Guid CustomerId);
        Task<Order?> GetCustomerOrderById(Guid CustomerId, Guid OrderId);
        Task AddPaymentAsync(Payment payment);
        Task SaveChangesAsync();
    }
}