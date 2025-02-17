using AspNetCoreEcommerce.Services.Contracts;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Repositories.Contracts;

namespace AspNetCoreEcommerce.Services.Implementations
{
    public class CartService(ICartRepository cartItemRepository) : ICartService
    {
        private readonly ICartRepository _cartItemRepository = cartItemRepository;
        public async Task<ReturnCartViewDto> ADddToCartAsync(Guid customerId, CartItemDto cartItemDto)
        {
            var cart = await _cartItemRepository.ADddToCartAsync(customerId, cartItemDto);
            return new ReturnCartViewDto
            {
                CartProductCount = cart.CartCount,
                CartTotalAmount = cart.CartPrice
            };
        }

        public async Task<ReturnCartViewDto> RemoveFromCartAsync(Guid customerId, Guid productId)
        {
            var cart = await _cartItemRepository.RemoveFromCartAsync(customerId, productId);
            return new ReturnCartViewDto
            {
                CartProductCount = cart.CartCount,
                CartTotalAmount = cart.CartPrice
            };
        }
    }
}