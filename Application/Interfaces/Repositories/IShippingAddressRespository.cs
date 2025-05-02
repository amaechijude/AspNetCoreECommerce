using AspNetCoreEcommerce.Domain.Entities;

namespace AspNetCoreEcommerce.Application.Interfaces.Repositories
{
    public interface IShippingAddressRespository
    {
        Task<ShippingAddress?> AddShippingAddress(ShippingAddress shippingAddress);
        Task<ShippingAddress?> GetShippingAddressByIdAsync(Guid customerId, Guid shippingAddId);
        Task<string?> DeleteShippingAddress(Guid customerId, Guid shippingid);
        Task SaveChangesAsync();
        Task<IEnumerable<ShippingAddress>> GetShippingAddressByUserId(Guid customerId);
    }
}
