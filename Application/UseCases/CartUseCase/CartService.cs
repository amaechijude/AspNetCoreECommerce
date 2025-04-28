// using AspNetCoreEcommerce

using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Application.Interfaces.Repositories;
using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Application.UseCases.CartUseCase
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartItemRepository;
        
        public CartService(ICartRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        public async Task<ResultPattern> ADddToCartAsync(Guid customerId, AddToCartDto addToCartDto)
        {
            var cart = await _cartItemRepository.ADddToCartAsync(customerId, addToCartDto);
            if (cart is null)
                return ResultPattern.FailResult("Product does not exist");

            var data = MapCartToDto(cart);
            return ResultPattern.SuccessResult(data, "Product added to cart successfully");
        }

        public async Task<ResultPattern> RemoveFromCartAsync(Guid customerId, Guid productId)
        {
            var cartItem = await _cartItemRepository.RemoveFromCartAsync(customerId, productId);
            if (cartItem is null)
                return ResultPattern.FailResult("Product does not exist in your Cart");
            
            return ResultPattern.SuccessResult("CaartItem Removed", "Product removed from cart successfully");
        }

        public async Task<ResultPattern> ViewCartAsync(Guid customerId)
        {
            var cart = await _cartItemRepository.GetOrCreateCartAsync(customerId);
            return ResultPattern.SuccessResult(MapCartToDto(cart), "Cart retrieved successfully");
        }

        private static CartViewDto MapCartToDto(Cart cart)
        {
            var cartview = new CartViewDto
            {
                CartId = cart.CartId,
                CustomerId = cart.CustomerId,
                CustomerEmail = cart.Customer.CustomerEmail ?? string.Empty,
                CustomerName = $"{cart.Customer.FirstName} {cart.Customer.LastName}" ?? string.Empty,
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
                        VendorName = ci.VendorName ?? string.Empty
                    })]
            };

            return cartview;
        }

    }
}