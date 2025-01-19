using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.Respositories.Contracts
{
    public interface ICartItemRepository
    {
        Task<CartItem> ADddToCartAsync(int customerID, int productId);
        Task RemoveFromCartAsync(int customerID, int productId);
    }
}