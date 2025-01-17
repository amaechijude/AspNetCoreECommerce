using DataTransferObjects;

namespace Services
{
    public interface ICustomerService
    {
        Task<CustomerDTO> CreateCustomerAsync(CustomerRegistrationDTO customer);
        Task<CustomerDTO> GetCustomerByIdAsync(int id);
        Task SaveChangesAsync();
    }
}
