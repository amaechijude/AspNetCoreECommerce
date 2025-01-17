using Entities;

namespace Repositories
{
    public interface ICartItemRepository
    {
        Task ADddToCartAsync(Product product);
        Task RemoveFromCartAsync(Product product);
    }
}