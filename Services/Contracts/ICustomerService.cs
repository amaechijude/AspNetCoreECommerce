using AspNetCoreEcommerce.DTOs;

namespace AspNetCoreEcommerce.Services.Contracts
{
    public interface ICustomerService
    {
        Task<CustomerDTO> CreateCustomerAsync(CustomerRegistrationDTO customer);
        Task<CustomerDTO> GetCustomerByIdAsync(int id);
        Task DeleteCustomerAsync(int customerId);
    }
}
