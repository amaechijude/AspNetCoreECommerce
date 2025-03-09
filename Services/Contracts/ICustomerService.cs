using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Result;

namespace AspNetCoreEcommerce.Services.Contracts
{
    public interface ICustomerService
    {
        Task<ResultPattern> CreateCustomerAsync(CustomerRegistrationDTO customer);
        Task<ResultPattern> GetCustomerByIdAsync(int id);
        Task DeleteCustomerAsync(int customerId);
        Task<ResultPattern> LoginCustomerAsync(LoginDto login);
    }
}
