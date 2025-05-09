// using AspNetCoreEcommerce

using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Application.Interfaces.Repositories;
using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Application.UseCases.CartUseCase
{
    public class CartService(ICartRepository cartItemRepository) : ICartService
    {
        private readonly ICartRepository _cartItemRepository = cartItemRepository;

        public async Task<ResultPattern> ADddToCartAsync(Guid userId, AddToCartDto addToCartDto, HttpRequest request)
        {
            var cart = await _cartItemRepository.ADddToCartAsync(userId, addToCartDto);
            if (cart is null)
                return ResultPattern.FailResult("Product does not exist");

            var data = MapCartToDto(cart, request);
            return ResultPattern.SuccessResult(data);
        }

        public async Task<ResultPattern> RemoveFromCartAsync(Guid userId, Guid productId)
        {
            var cartItem = await _cartItemRepository.RemoveFromCartAsync(userId, productId);
            if (cartItem is null)
                return ResultPattern.FailResult("Product does not exist in your Cart");
            
            return ResultPattern.SuccessResult("Product removed from cart successfully");
        }

        public async Task<ResultPattern> ViewCartAsync(Guid userId, HttpRequest request)
        {
            var cart = await _cartItemRepository.GetOrCreateCartAsync(userId);
            if (cart is null)
                return ResultPattern.FailResult("Cart does not exist");
            return ResultPattern.SuccessResult(MapCartToDto(cart, request));
        }

        private static CartViewDto MapCartToDto(Cart cart, HttpRequest request)
        {
            var cartview = new CartViewDto
            {
                CartId = cart.CartId,
                UserId = cart.UserId,
                UserEmail = cart.User.Email ?? string.Empty,
                UserName = $"{cart.User.UserName}",
                CartProductCount = cart.CartItemsCount,
                CartTotalAmount = cart.CartTotalAmount,
                CartItems = [.. cart.CartItems.Select(ci => new CartItemViewDto
                    {
                        CartItemId = ci.CartItemId,
                        UnitPrice = ci.UnitPrice,
                        Quantity = ci.Quantity,
                        TotalPrice = ci.UnitPrice * ci.Quantity,
                        ProductId = ci.ProductId,
                        ProductName = ci.ProductName ?? string.Empty,
                        ProductImageUrl = GlobalConstants.GetImagetUrl(request, ci.Product.ImageUrl),
                        VendorId = ci.VendorId,
                        VendorName = ci.VendorName ?? string.Empty
                    })]
            };

            return cartview;
        }

    }
}