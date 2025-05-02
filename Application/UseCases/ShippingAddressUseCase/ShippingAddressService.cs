using AspNetCoreEcommerce.Application.Interfaces.Repositories;
using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Application.UseCases.ShippingAddressUseCase
{
    public class ShippingAddressService(
        IShippingAddressRespository shippingAddressRespository
        ) : IShippingAddressService
    {
        private readonly IShippingAddressRespository _shippingAddressRespository = shippingAddressRespository;

        public async Task<ResultPattern> AddShippingAddressAsync(User user, ShippingAddressDto shippingAddress)
        {
            var shippingAddressEntity = CreatePrivatesShAdrr(user, shippingAddress);

            var result = await _shippingAddressRespository.AddShippingAddress(shippingAddressEntity);
            if (result is null)
                return ResultPattern.FailResult("Error adding shipping address");

            return ResultPattern.SuccessResult(MapShippingAddress(result));
        }

        public async Task<ResultPattern> DeleteShippingAddressAsync(Guid userId, Guid shippingId)
        {
            var data = await _shippingAddressRespository.DeleteShippingAddress(userId, shippingId);
            if (data is null)
                return ResultPattern.FailResult("Shipping address not found");

            return ResultPattern.SuccessResult(data);
        }

        public async Task<ResultPattern> GetShippingAddressByUserIdAsync(Guid customerId)
        {
            var shippingAddresses = await _shippingAddressRespository
                .GetShippingAddressByUserId(customerId);
            if (shippingAddresses is null)
                return ResultPattern.FailResult("Shipping addresses not found");
            return ResultPattern
                .SuccessResult(
                    shippingAddresses.Select(sh => MapShippingAddress(sh))
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
                    MapShippingAddress(shippingAddress)
                 );
        }

        private static ShippingAddressViewDto MapShippingAddress(ShippingAddress shippingAddress)
        {
            return new ShippingAddressViewDto
            { 
                ShippingAddressId = shippingAddress.ShippingAddressId,
                UserId = shippingAddress.UserId,
                UserName = $"{shippingAddress.FirstName} {shippingAddress.LastName}",
                ShippingAddressName = $"{shippingAddress.FirstName} {shippingAddress.LastName}",
                ShippingAddressPhone = shippingAddress.PhoneNumber,
                AddressOne = shippingAddress.AddressLine1,
                SecondAddress = shippingAddress.AddressLine2,
                City = shippingAddress.City,
                State = shippingAddress.State,
                Country = shippingAddress.Country,
                PostalCode = shippingAddress.ZipCode,
            };
        }

        private static ShippingAddress CreatePrivatesShAdrr(User user, ShippingAddressDto shippingAddress)
        {
            return new ShippingAddress
            {
                ShippingAddressId = Guid.CreateVersion7(),
                FirstName = shippingAddress.FirstName,
                LastName = shippingAddress.LastName,
                PhoneNumber = shippingAddress.Phone,
                AddressLine1 = shippingAddress.AddressLine1,
                AddressLine2 = shippingAddress.AddressLine2,
                City = shippingAddress.City,
                State = shippingAddress.State,
                Country = shippingAddress.Country,
                ZipCode = shippingAddress.PostalCode,
                UserId = user.Id,
                User = user
            };
        }
    }
}
