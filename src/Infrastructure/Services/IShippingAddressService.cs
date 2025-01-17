using DataTransferObjects;

namespace Services
{
    public interface IShippingAddressService
    {
        Task AddShippingAddressAsync(ShippingAddressDto shippingAddress);
        Task RemoveShippingAddressAsync(int shippingAddressId);
    }
}
