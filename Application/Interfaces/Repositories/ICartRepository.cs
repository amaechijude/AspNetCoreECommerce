using AspNetCoreEcommerce.Domain.Entities;

namespace AspNetCoreEcommerce.Application.Interfaces.Repositories
{
    public interface ICartRepository
    {
        Task<bool> ADddToCartAsync(Guid userId, Guid productId, int quantity);
        Task<bool> RemoveFromCartAsync(Guid customerID, Guid productId);
        Task<Cart> ViewCartAsync(Guid userId);
        Task SaveChangesAsync();
    }
}