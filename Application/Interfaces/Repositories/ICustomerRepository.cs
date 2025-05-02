using AspNetCoreEcommerce.Domain.Entities;

namespace AspNetCoreEcommerce.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User customer);
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByUserIdAsync(Guid id);
        Task<string?> DeleteUserAsync(Guid customerId);
        Task<User?> GetUserByEmailAsync(string email);
        Task SaveChangesAsync();
    }
}