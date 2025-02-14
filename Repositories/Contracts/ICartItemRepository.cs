using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.Respositories.Contracts
{
    public interface ICartItemRepository
    {
        Task<CartItem> ADddToCartAsync(Guid customerID, Guid productId);
        Task<CartItem> RemoveFromCartAsync(Guid customerID, Guid productId);
    }
}