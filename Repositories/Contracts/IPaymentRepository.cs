using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.Repositories.Contracts
{
    public interface IPaymentRepository
    {
        Task<(Customer, Order)> GetCustomerAndIdAsync(Guid CustomerId, Guid OrderId);
        Task AddPaymentAsync(Payment payment);
    }
}