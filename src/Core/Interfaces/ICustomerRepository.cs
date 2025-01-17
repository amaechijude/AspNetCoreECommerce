using DataTransferObjects;

namespace Interfaces
{
    public interface ICustomerRepository
    {
        Task<string> CreateCustomerAsync(CustomerRegistrationDTO customer);
        Task<CustomerDTO> GetCustomerByIdAsync(int id);
        Task SaveChangesAsync();
    }
}