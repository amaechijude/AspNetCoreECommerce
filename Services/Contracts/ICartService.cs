using AspNetCoreEcommerce.DTOs;

namespace AspNetCoreEcommerce.Services.Contracts
{
    public interface ICartService
    {
        Task<CartViewDto> ADddToCartAsync(Guid customerId, AddToCartDto addToCartDto);
        Task<RemoveFromCartViewDto> RemoveFromCartAsync(Guid customerId, Guid productId);
        Task<CartViewDto> ViewCartAsync(Guid customerId);
    }
}
