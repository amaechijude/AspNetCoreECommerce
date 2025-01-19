using Entities;

namespace Repositories
{
    public interface ICartItemRepository
    {
        Task<CartItem> GetOrCreateCartItemAsync(int customerId);
        Task<CartItem> ADddToCartAsync(Customer customer, Product product);
        Task RemoveFromCartAsync(Customer customer, Product product);
    }
}