using AspNetCoreEcommerce.Services.Contracts;
using AspNetCoreEcommerce.Respositories.Contracts;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.Services.Implementations
{
    public class CartItemService(ICartItemRepository cartItemRepository) : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository = cartItemRepository;
        public async Task<CartItemViewDto> ADddToCartAsync(Guid customerId, Guid productId, HttpRequest request)
        {
                var cartItem = await _cartItemRepository.ADddToCartAsync(customerId, productId);

                return MapToCartItemDto(cartItem, request);
        }
        public async Task<CartItemViewDto> RemoveFromCartAsync(Guid customerId, Guid productId, HttpRequest request)
        {
                var cartItem = await _cartItemRepository.RemoveFromCartAsync(customerId, productId);

                return MapToCartItemDto(cartItem, request);
        }

        private static CartItemViewDto MapToCartItemDto(CartItem cartItem, HttpRequest request)
        {
            return new CartItemViewDto
            {
                CustomerId = cartItem.CustomerId,
                CustomerName = cartItem.Customer.CustomerName,
                TotalPrice = cartItem.TotalPrice,
                Products = [.. cartItem.Products.Select(p => new ProductViewDto{
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    ImageUrl = GlobalConstants.GetImagetUrl(request, p.ImageName),
                    Price = p.Price,
                    VendorId = p.VendorId,
                    VendorName = p.Vendor.VendorName
                })]
            };
        }
    }
}
