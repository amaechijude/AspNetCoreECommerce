using AspNetCoreEcommerce.Application.UseCases.Authentication;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Application.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<ResultPattern> CreateCustomerAsync(User user, string firstname, string lastName);
        Task<ResultPattern> GetCustomerByIdAsync(Guid id);
        Task<ResultPattern> DeleteCustomerAsync(Guid customerId);
    }
}
