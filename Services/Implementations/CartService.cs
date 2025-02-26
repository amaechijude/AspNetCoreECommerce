using AspNetCoreEcommerce.Services.Contracts;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Repositories.Contracts;
using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.Services.Implementations
{
    public class CartService(ICartRepository cartItemRepository) : ICartService
    {
        private readonly ICartRepository _cartItemRepository = cartItemRepository;
        public async Task<CartViewDto> ADddToCartAsync(Guid customerId, AddToCartDto addToCartDto)
        {
            var cart = await _cartItemRepository.ADddToCartAsync(customerId, addToCartDto);
            return MapCartToDto(cart);
        }

        public async Task<RemoveFromCartViewDto> RemoveFromCartAsync(Guid customerId, Guid productId)
        {
            var cartItem = await _cartItemRepository.RemoveFromCartAsync(customerId, productId);
            
            return MapRemoveDto(cartItem);
        }

        public async Task<CartViewDto> ViewCartAsync(Guid customerId)
        {
            var cart = await _cartItemRepository.GetOrCreateCartAsync(customerId);
            return MapCartToDto(cart);
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

        private static RemoveFromCartViewDto MapRemoveDto(CartItem? cartItem)
        {
            if (cartItem is null)
            {
                return new RemoveFromCartViewDto
                {
                    Success = false,
                    Message = "Product does not exist in your Cart",
                };
            }
            else
            {
                return new RemoveFromCartViewDto
                {
                    Success = true,
                    Message = "Product removed from your Cart",
                };
            }
        }
    }
}