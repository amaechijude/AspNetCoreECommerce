using AspNetCoreEcommerce.Application.Interfaces.Repositories;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreEcommerce.Infrastructure.Repositories
{
    public class ShippingAddressRespository(
        ApplicationDbContext context,
        ILogger<ShippingAddressRespository> logger
        ) : IShippingAddressRespository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger<ShippingAddressRespository> _logger = logger;

        public async Task<ShippingAddress?> AddShippingAddress(ShippingAddress shippingAddress)
        {
            try
            {
                await _context.ShippingAddresses.AddAsync(shippingAddress);
                await _context.SaveChangesAsync();
                return shippingAddress;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error adding shipping address");
                return null;
            }
        }

        public async Task<string?> DeleteShippingAddress(Guid customerId, Guid shippingid)
        {
            var shippingAddress = await _context.ShippingAddresses
                .Where(sh => sh.ShippingAddressId == shippingid && sh.UserId == customerId)
                .FirstOrDefaultAsync();
            if (shippingAddress == null)
                return null;

            _context.ShippingAddresses.Remove(shippingAddress);
            await _context.SaveChangesAsync();
            return "Shipping address removed";
        }

        public async Task<IEnumerable<ShippingAddress>> GetShippingAddressByUserId(Guid userId)
        {
            return await _context.ShippingAddresses
                .Where(sh => sh.UserId == userId)
                .OrderByDescending(s  => s.ShippingAddressId)
                .ToListAsync();
        }

        public async Task<ShippingAddress?> GetShippingAddressByIdAsync(Guid userId, Guid shippingAddId)
        {
            return await _context.ShippingAddresses
                .Where(sh => sh.UserId == userId && sh.ShippingAddressId == shippingAddId)
                .FirstOrDefaultAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
