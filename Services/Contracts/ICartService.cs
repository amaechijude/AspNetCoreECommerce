using AspNetCoreEcommerce.DTOs;

namespace AspNetCoreEcommerce.Services.Contracts
{
    public interface ICartService
    {
        Task<ReturnCartViewDto> ADddToCartAsync(Guid customerId, CartItemDto cartItemDto);
        Task<ReturnCartViewDto> RemoveFromCartAsync(Guid customerId, Guid productId);
        Task<CustomerCartViewDto> ViewCartAsync(Guid customerId);
    }
}
