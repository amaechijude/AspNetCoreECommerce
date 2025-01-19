using AspNetCoreEcommerce.DTOs;

namespace AspNetCoreEcommerce.Services.Contracts
{
    public interface IShippingAddressService
    {
        Task AddShippingAddressAsync(ShippingAddressDto shippingAddress);
        Task RemoveShippingAddressAsync(int shippingAddressId);
    }
}
