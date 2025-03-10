using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.Repositories.Contracts
{
    public interface ICustomerRepository
    {
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task<Customer?> GetCustomerByIdAsync(Guid id);
        Task<string?> DeleteCustomerAsync(Guid customerId);
        Task<Customer?> GetCustomerByEmailAsync(string email);
        Task SaveChangesAsync();
    }
}