
namespace Services
{
    public interface ICartItemService
    {
        Task ADddToCartAsync(int productId);
        Task RemoveFromCartAsync(int productId);
    }
}
