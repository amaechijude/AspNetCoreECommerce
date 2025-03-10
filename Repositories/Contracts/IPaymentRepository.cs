using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.Repositories.Contracts
{
    public interface IPaymentRepository
    {
        Task<Customer?> GetCustomerByIdAsync(Guid CustomerId);
        Task<Order?> GetCustomerOrderById(Guid CustomerId, Guid OrderId);
        Task AddPaymentAsync(Payment payment);
    }
}