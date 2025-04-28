using AspNetCoreEcommerce.Application.UseCases.ShippingAddressUseCase;
using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Application.Interfaces.Services
{
    public interface IShippingAddressService
    {
        Task<ResultPattern> AddShippingAddressAsync(Guid customerId, ShippingAddressDto shippingAddress);
        Task<ResultPattern> DeleteShippingAddressAsync(Guid customerid, Guid shippingId);
        Task<ResultPattern> GetShippingAddressByCustomerIdAsync(Guid customerId);
        Task<ResultPattern> GetShippingAddressByIdAsync(Guid customerId, Guid shippingAddId);
    }
}
