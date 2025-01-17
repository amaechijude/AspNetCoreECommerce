using Entities;

namespace Repositories
{
    public interface IShippingAddressRepository
    {
        Task AddShippingAddressAsync(ShippingAddress shippingAddress);
        Task RemoveShippingAddressAsync(int shippingAddressId);
        
    }
}