using AspNetCoreEcommerce.Application.UseCases.CartUseCase;
using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Application.Interfaces.Services
{
    public interface ICartService
    {
        Task<ResultPattern> ADddToCartAsync(Guid customerId, AddToCartDto addToCartDto);
        Task<ResultPattern> RemoveFromCartAsync(Guid customerId, Guid productId);
        Task<ResultPattern> ViewCartAsync(Guid customerId);
    }
}
