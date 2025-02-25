using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Repositories.Contracts;
using AspNetCoreEcommerce.Services.Contracts;

namespace AspNetCoreEcommerce.Services.Implementations
{
    public class ShippingAddressService(IShippingAddressRespository shippingAddressRespository) : IShippingAddressService
    {
        private readonly IShippingAddressRespository _shippingAddressRespository = shippingAddressRespository;

        public async Task<ShippingAddressDto> AddShippingAddressAsync(Guid customerId, ShippingAddressDto shippingAddress)
        {
            var customer = await _shippingAddressRespository.GetCustomerByIdAsync(customerId);
            var shippingAddressEntity = new ShippingAddress
            {
                Id = Guid.CreateVersion7(),
                Email = customer.CustomerEmail,
                FirstName= shippingAddress.FirstName,
                LastName = shippingAddress.LastName,
                Phone = shippingAddress.Phone,
                AddressOne = shippingAddress.AddressOne,
                SecondAddress = shippingAddress.SecondAddress,
                City = shippingAddress.City,
                State = shippingAddress.State,
                Country = shippingAddress.Country,
                PostalCode = shippingAddress.PostalCode,
                CustormerId = customer.CustomerID,
                Customer = customer
            };
            var result = await _shippingAddressRespository.AddShippingAddress(shippingAddressEntity);
            return new ShippingAddressDto
            {
                AddressOne = result.AddressOne,
                City = result.City,
                Country = result.Country,
                PostalCode = result.PostalCode,
                State = result.State,
                //CustomerId = result.CustormerId
            };
        }

        public async Task DeleteShippingAddressAsync(Guid customerId, Guid shippingId)
        {
            var customer = await _shippingAddressRespository.GetCustomerByIdAsync(customerId);
            await _shippingAddressRespository.DeleteShippingAddress(customer.CustomerID, shippingId);
        }

        public async Task<IEnumerable<ShippingAddressDto>> GetShippingAddressByCustomerIdAsync(Guid customerId)
        {
            var customer = await _shippingAddressRespository.GetCustomerByIdAsync(customerId);
            var shippingAddresses = await _shippingAddressRespository.GetShippingAddressByCustomerId(customer.CustomerID);
            return shippingAddresses.Select(sh => new ShippingAddressDto
            {
                AddressOne = sh.AddressOne,
                City = sh.City,
                Country = sh.Country,
                PostalCode = sh.PostalCode,
                State = sh.State,
                //CustomerId = sh.CustormerId
            });
        }
    }
}
