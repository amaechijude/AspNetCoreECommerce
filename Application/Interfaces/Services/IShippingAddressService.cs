using AspNetCoreEcommerce.Application.UseCases.ShippingAddressUseCase;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Application.Interfaces.Services
{
    public interface IShippingAddressService
    {
        Task<ResultPattern> AddShippingAddressAsync(User user, ShippingAddressDto shippingAddress);
        Task<ResultPattern> DeleteShippingAddressAsync(Guid customerid, Guid shippingId);
        Task<ResultPattern> GetShippingAddressByUserIdAsync(Guid customerId);
        Task<ResultPattern> GetShippingAddressByIdAsync(Guid customerId, Guid shippingAddId);
    }
}
