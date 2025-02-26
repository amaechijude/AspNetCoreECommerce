using AspNetCoreEcommerce.DTOs;

namespace AspNetCoreEcommerce.Services.Contracts
{
    public interface IShippingAddressService
    {
        Task<ShippingAddressViewDto> AddShippingAddressAsync(Guid customerId, ShippingAddressDto shippingAddress);
        Task DeleteShippingAddressAsync(Guid customerid, Guid shippingId);
        Task<IEnumerable<ShippingAddressViewDto>> GetShippingAddressByCustomerIdAsync(Guid customerId);
    }
}
