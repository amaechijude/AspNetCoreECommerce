using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.Repositories.Contracts
{
    public interface ICartRepository
    {
        Task<Cart> GetCustomerCartAsync(Guid customerId);
        Task<Cart> ADddToCartAsync(Guid customerId, AddToCartDto addToCartDto);
        Task<Cart> RemoveFromCartAsync(Guid customerID, Guid productId);
        Task<Cart> GetOrCreateCartAsync(Guid customerId);
    }
}