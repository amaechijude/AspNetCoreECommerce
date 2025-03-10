using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.ResultResponse;

namespace AspNetCoreEcommerce.Services.Contracts
{
    public interface ICartService
    {
        Task<ResultPattern> ADddToCartAsync(Guid customerId, AddToCartDto addToCartDto);
        Task<ResultPattern> RemoveFromCartAsync(Guid customerId, Guid productId);
        Task<ResultPattern> ViewCartAsync(Guid customerId);
    }
}
