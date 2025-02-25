using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.Repositories.Contracts
{
    public interface IShippingAddressRespository
    {
        Task<ShippingAddress> AddShippingAddress(ShippingAddress shippingAddress);
        Task<IEnumerable<ShippingAddress>> GetShippingAddressByCustomerId(Guid customerId);
        Task DeleteShippingAddress(Guid customerId, Guid shippingid);
        Task<Customer> GetCustomerByIdAsync(Guid customerId);
    }
}
