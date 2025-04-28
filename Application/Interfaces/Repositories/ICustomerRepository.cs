using AspNetCoreEcommerce.Domain.Entities;

namespace AspNetCoreEcommerce.Application.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task<Customer?> GetCustomerByIdAsync(Guid id);
        Task<Customer?> GetCustomerByUserIdAsync(Guid id);
        Task<string?> DeleteCustomerAsync(Guid customerId);
        Task<Customer?> GetCustomerByEmailAsync(string email);
        Task SaveChangesAsync();
    }
}