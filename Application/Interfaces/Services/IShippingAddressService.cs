﻿using AspNetCoreEcommerce.Application.UseCases.ShippingAddressUseCase;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Application.Interfaces.Services
{
    public interface IShippingAddressService
    {
        Task<ResultPattern> AddShippingAddressAsync(User user, AddShippingAddressDto shippingAddress);
        Task<ResultPattern> DeleteShippingAddressAsync(Guid customerid, Guid shippingId);
        Task<IEnumerable<ShippingAddressViewDto>> GetShippingAddressByUserIdAsync(Guid userId);
        Task<ResultPattern> GetShippingAddressByIdAsync(Guid customerId, Guid shippingAddId);
    }
}
