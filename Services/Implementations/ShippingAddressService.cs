using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Repositories.Contracts;
using AspNetCoreEcommerce.ResultResponse;
using AspNetCoreEcommerce.Services.Contracts;

namespace AspNetCoreEcommerce.Services.Implementations
{
    public class ShippingAddressService(IShippingAddressRespository shippingAddressRespository) : IShippingAddressService
    {
        private readonly IShippingAddressRespository _shippingAddressRespository = shippingAddressRespository;

        public async Task<ResultPattern> AddShippingAddressAsync(Guid customerId, ShippingAddressDto shippingAddress)
        {
            var customer = await _shippingAddressRespository.GetCustomerByIdAsync(customerId);
            if (customer == null)
                return ResultPattern.FailResult("User not found");
            var shippingAddressEntity = CreatePrivatesShAdrr(customer, shippingAddress);

            var result = await _shippingAddressRespository.AddShippingAddress(shippingAddressEntity);
            if (result is null)
                return ResultPattern.FailResult("Error adding shipping address");

            return ResultPattern.SuccessResult(result, "Shipping address added successfully");
        }

        public async Task<ResultPattern> DeleteShippingAddressAsync(Guid customerId, Guid shippingId)
        {
            var customer = await _shippingAddressRespository
                .GetCustomerByIdAsync(customerId);
            if (customer is null)
                return ResultPattern.FailResult("User not found");
            var data = await _shippingAddressRespository.DeleteShippingAddress(customer.CustomerID, shippingId);
            if (data is null)
                return ResultPattern.FailResult("Shipping address not found");

            return ResultPattern.SuccessResult(data, "Shipping address removed");
        }

        public async Task<ResultPattern> GetShippingAddressByCustomerIdAsync(Guid customerId)
        {
            var shippingAddresses = await _shippingAddressRespository
                .GetShippingAddressByCustomerId(customerId);
            if (shippingAddresses is null)
                return ResultPattern.FailResult("Shipping addresses not found");
            return ResultPattern
                .SuccessResult(
                    shippingAddresses.Select(sh => MapShippingAddress(sh)),
                    "Fetched shipping Address"
                 );
        }

        public async Task<ResultPattern> GetShippingAddressByIdAsync(Guid customerId, Guid shippingAddId)
        {
            var shippingAddress = await _shippingAddressRespository
                .GetShippingAddressByIdAsync(customerId, shippingAddId);
            if (shippingAddress is null)
                return ResultPattern.FailResult("Shipping address not found");
            return ResultPattern
                .SuccessResult(
                    MapShippingAddress(shippingAddress),
                    "Fetched shipping Address"
                 );
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

        private static ShippingAddress CreatePrivatesShAdrr(Customer customer, ShippingAddressDto shippingAddress)
        {
            return new ShippingAddress
            {
                ShippingAddressId = Guid.CreateVersion7(),
                Email = customer.CustomerEmail,
                FirstName = shippingAddress.FirstName,
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
        }
    }
}
