using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.Respositories.Contracts
{
    public interface ICustomerRepository
    {
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task DeleteCustomerAsync(int customerId);
        Task<Customer?> GetCustomerByEmailAsync(string email);
        Task SaveLastLoginDate();
    }
}