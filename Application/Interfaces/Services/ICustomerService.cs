using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<ResultPattern> CreateUserAsync(User user, string firstname, string lastName);
        Task<ResultPattern> GetUserByIdAsync(Guid id);
        Task<ResultPattern> DeleteUserAsync(Guid customerId);
    }
}
