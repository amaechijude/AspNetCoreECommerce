using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.Repositories.Contracts
{
    public interface ICartRepository
    {
        Task<Cart> ADddToCartAsync(Guid customerId, CartItemDto cartItemDto);
        Task<Cart> RemoveFromCartAsync(Guid customerID, Guid productId);
    }
}