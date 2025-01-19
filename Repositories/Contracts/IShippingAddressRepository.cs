using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.Respositories.Contracts
{
    public interface IShippingAddressRepository
    {
        Task AddShippingAddressAsync(ShippingAddress shippingAddress);
        Task RemoveShippingAddressAsync(int shippingAddressId);
        
    }
}