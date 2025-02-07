using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.Services.Contracts
{
    public interface ICustomerService
    {
        Task<CustomerDTO> CreateCustomerAsync(CustomerRegistrationDTO customer);
        Task<CustomerDTO> GetCustomerByIdAsync(int id);
        Task DeleteCustomerAsync(int customerId);
        Task<CustomerLoginViewDto> LoginCustomerAsync(LoginDto login);
    }
}
