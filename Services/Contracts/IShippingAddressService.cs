using AspNetCoreEcommerce.DTOs;

namespace AspNetCoreEcommerce.Services.Contracts
{
    public interface IShippingAddressService
    {
        Task<ShippingAddressDto> AddShippingAddressAsync(Guid customerId, ShippingAddressDto shippingAddress);
        Task DeleteShippingAddressAsync(Guid customerid, Guid shippingId);
        Task<IEnumerable<ShippingAddressDto>> GetShippingAddressByCustomerIdAsync(Guid customerId);
    }
}
