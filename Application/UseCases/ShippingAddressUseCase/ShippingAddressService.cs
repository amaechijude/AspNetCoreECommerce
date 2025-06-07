using AspNetCoreEcommerce.Application.Interfaces.Repositories;
using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Shared;
using Microsoft.Extensions.Caching.Memory;

namespace AspNetCoreEcommerce.Application.UseCases.ShippingAddressUseCase
{
    public class ShippingAddressService(
        IShippingAddressRespository shippingAddressRespository,
        IMemoryCache memoryCache
        ) : IShippingAddressService
    {
        private readonly IShippingAddressRespository _shippingAddressRespository = shippingAddressRespository;
        private readonly IMemoryCache _memoryCache = memoryCache;

        public async Task<ResultPattern> AddShippingAddressAsync(User user, AddShippingAddressDto shippingAddress)
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

        public async Task<IEnumerable<ShippingAddressViewDto>> GetShippingAddressByUserIdAsync(Guid userId)
        {
            string cacheKey = $"ShipListFor_{userId}";
            bool inCache = _memoryCache.TryGetValue(cacheKey, out IEnumerable<ShippingAddress>? cacheData);
            if (inCache && cacheData is not null)
            {
                return [..cacheData
                .Select(sh => MapShippingAddress(sh))];
            }
            IEnumerable<ShippingAddress> shippingAddresses = await _shippingAddressRespository
                .GetShippingAddressByUserId(userId);

            // Add to cache
            _memoryCache.Set(cacheKey, shippingAddresses, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(30),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
            return [..shippingAddresses
                .Select(sh => MapShippingAddress(sh))];
        }

        public async Task<ResultPattern> GetShippingAddressByIdAsync(Guid customerId, Guid shippingAddId)
        {
            string cacheKey = $"Shipping_{shippingAddId}";
            bool inCache = _memoryCache.TryGetValue(cacheKey, out ShippingAddress? cacheData);
            if (inCache && cacheData is not null)
            {
                return ResultPattern
                .SuccessResult(
                    MapShippingAddress(cacheData)
                 );
            }
            ShippingAddress? shippingAddress = await _shippingAddressRespository
                .GetShippingAddressByIdAsync(customerId, shippingAddId);

            if (shippingAddress is null)
                return ResultPattern.FailResult("Shipping address not found");

            _memoryCache.Set(cacheKey, shippingAddress, new MemoryCacheEntryOptions
            { 
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20)
            });
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

        private static ShippingAddress CreatePrivatesShAdrr(User user, AddShippingAddressDto shippingAddress)
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
