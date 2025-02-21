using AspNetCoreEcommerce.Services.Contracts;
using AspNetCoreEcommerce.DTOs;
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
                CartProductCount = cart.CartItemsCount,
                CartTotalAmount = cart.CartTotalAmount
            };
        }

        public async Task<ReturnCartViewDto> RemoveFromCartAsync(Guid customerId, Guid productId)
        {
            var cart = await _cartItemRepository.RemoveFromCartAsync(customerId, productId);
            return new ReturnCartViewDto
            {
                CartProductCount = cart.CartItemsCount,
                CartTotalAmount = cart.CartTotalAmount
            };
        }

        public async Task<CustomerCartViewDto> ViewCartAsync(Guid customerId)
        {
            var cart = await _cartItemRepository.GetOrCreateCartAsync(customerId);
            return new CustomerCartViewDto
            {
                CartProductCount = cart.CartItemsCount,
                CartTotalAmount = cart.CartTotalAmount,
                CartItems = cart.CartItems.Select(ci => new CsCartItemViewDto
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.ProductName,
                    Price = ci.Product.Price,
                    Quantity = ci.Quantity,
                    SubTotalPrice = ci.Product.Price * ci.Quantity
                }).ToList()
            };
        }
    }
}