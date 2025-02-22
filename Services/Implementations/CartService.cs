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

        public async Task<CartViewDto> RemoveFromCartAsync(Guid customerId, Guid productId)
        {
            var cart = await _cartItemRepository.RemoveFromCartAsync(customerId, productId);
            return MapCartToDto(cart);
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
                CartProductCount = cart.CartItemsCount,
                CartTotalAmount = cart.CartTotalAmount,
                CartItems = cart.CartItems.Select(ci => new CartItemViewDto
                    {
                        CartItemId = ci.CartItemId,
                        UnitPrice = ci.UnitPrice,
                        Quantity = ci.Quantity,
                        TotalPrice = ci.UnitPrice * ci.Quantity,
                        Product = new ProductViewDto
                            {
                                ProductId = ci.ProductId,
                                ProductName = ci.Product.ProductName,
                                Description = ci.Product.Description,
                                ImageUrl = ci.Product.ImageUrl,
                                Price = ci.Product.Price,
                                VendorId = ci.Product.VendorId,
                                VendorName = ci.Product.Vendor.VendorName
                            }
                    }).ToList()
            };

            return cartview;
        }
    }
}