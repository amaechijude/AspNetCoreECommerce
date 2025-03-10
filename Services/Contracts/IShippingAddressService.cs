using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.ResultResponse;

namespace AspNetCoreEcommerce.Services.Contracts
{
    public interface IShippingAddressService
    {
        Task<ResultPattern> AddShippingAddressAsync(Guid customerId, ShippingAddressDto shippingAddress);
        Task<ResultPattern> DeleteShippingAddressAsync(Guid customerid, Guid shippingId);
        Task<ResultPattern> GetShippingAddressByCustomerIdAsync(Guid customerId);
        Task<ResultPattern> GetShippingAddressByIdAsync(Guid customerId, Guid shippingAddId);
    }
}
