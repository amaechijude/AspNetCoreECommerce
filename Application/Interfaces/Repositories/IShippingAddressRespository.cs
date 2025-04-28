using AspNetCoreEcommerce.Domain.Entities;

namespace AspNetCoreEcommerce.Application.Interfaces.Repositories
{
    public interface IShippingAddressRespository
    {
        Task<ShippingAddress?> AddShippingAddress(ShippingAddress shippingAddress);
        Task<IEnumerable<ShippingAddress>> GetShippingAddressByCustomerId(Guid customerId);
        Task<ShippingAddress?> GetShippingAddressByIdAsync(Guid customerId, Guid shippingAddId);
        Task<string?> DeleteShippingAddress(Guid customerId, Guid shippingid);
        Task<Customer?> GetCustomerByIdAsync(Guid customerId);
        Task IUnitOfWork();
    }
}
