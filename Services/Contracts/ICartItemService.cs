using AspNetCoreEcommerce.DTOs;

namespace AspNetCoreEcommerce.Services.Contracts
{
    public interface ICartItemService
    {
        public Task<CartItemViewDto> ADddToCartAsync(Guid customerId, Guid productId, HttpRequest request);
        public Task<CartItemViewDto> RemoveFromCartAsync(Guid customerId, Guid productId, HttpRequest request);
    }
}
