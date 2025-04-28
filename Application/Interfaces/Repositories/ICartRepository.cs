using AspNetCoreEcommerce.Application.UseCases.CartUseCase;
using AspNetCoreEcommerce.Domain.Entities;

namespace AspNetCoreEcommerce.Application.Interfaces.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> ADddToCartAsync(Guid customerId, AddToCartDto addToCartDto);
        Task<CartItem?> RemoveFromCartAsync(Guid customerID, Guid productId);
        Task<Cart> GetOrCreateCartAsync(Guid customerId);
        Task IUnitOfWork();
    }
}