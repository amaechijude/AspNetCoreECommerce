using AspNetCoreEcommerce.Services.Contracts;
using AspNetCoreEcommerce.Respositories.Contracts;
using AspNetCoreEcommerce.Respositories.Implementations;

namespace AspNetCoreEcommerce.Services.Implementations
{
    public class CartItemService(ICartItemRepository cartItemRepository) : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository = cartItemRepository;
        public async Task ADddToCartAsync(int customerId, int productId)
        {
                var cartItem = await _cartItemRepository.ADddToCartAsync(customerId, productId);
        }
        public async Task RemoveFromCartAsync(int customerId, int productId)
        {
                await _cartItemRepository.RemoveFromCartAsync(customerId, productId);
        }
    }
}
