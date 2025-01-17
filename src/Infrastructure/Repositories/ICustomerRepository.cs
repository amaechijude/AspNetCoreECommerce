using Entities;

namespace Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task<Customer> GetCustomerByIdAsync(int id);
        Task SaveChangesAsync();
    }
}