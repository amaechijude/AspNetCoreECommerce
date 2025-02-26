using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Repositories.Contracts;
using AspNetCoreEcommerce.Services.Contracts;

namespace AspNetCoreEcommerce.Services.Implementations
{
    public class ShippingAddressService(IShippingAddressRespository shippingAddressRespository) : IShippingAddressService
    {
        private readonly IShippingAddressRespository _shippingAddressRespository = shippingAddressRespository;

        public async Task<ShippingAddressViewDto> AddShippingAddressAsync(Guid customerId, ShippingAddressDto shippingAddress)
        {
            var customer = await _shippingAddressRespository.GetCustomerByIdAsync(customerId);
            var shippingAddressEntity = new ShippingAddress
            {
                ShippingAddressId = Guid.CreateVersion7(),
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
                CustomerId = customer.CustomerID,
                Customer = customer
            };
            var result = await _shippingAddressRespository.AddShippingAddress(shippingAddressEntity);
            return MapShippingAddress(result);
        }

        public async Task DeleteShippingAddressAsync(Guid customerId, Guid shippingId)
        {
            var customer = await _shippingAddressRespository.GetCustomerByIdAsync(customerId);
            await _shippingAddressRespository.DeleteShippingAddress(customer.CustomerID, shippingId);
        }

        public async Task<IEnumerable<ShippingAddressViewDto>> GetShippingAddressByCustomerIdAsync(Guid customerId)
        {
            var customer = await _shippingAddressRespository.GetCustomerByIdAsync(customerId);
            var shippingAddresses = await _shippingAddressRespository.GetShippingAddressByCustomerId(customer.CustomerID);
            return shippingAddresses.Select(sh => MapShippingAddress(sh));
        }

        private static ShippingAddressViewDto MapShippingAddress(ShippingAddress shippingAddress)
        {
            return new ShippingAddressViewDto
            { 
                ShippingAddressId = shippingAddress.ShippingAddressId,
                CustomerId = shippingAddress.CustomerId,
                CustomerName = $"{shippingAddress.Customer.FirstName} {shippingAddress.Customer.LastName}",
                ShippingAddressName = $"{shippingAddress.FirstName} {shippingAddress.LastName}",
                ShippingAddressPhone = shippingAddress.Phone,
                AddressOne = shippingAddress.AddressOne,
                SecondAddress = shippingAddress.SecondAddress,
                City = shippingAddress.City,
                State = shippingAddress.State,
                Country = shippingAddress.Country,
                PostalCode = shippingAddress.PostalCode,
            };
        }
    }
}
