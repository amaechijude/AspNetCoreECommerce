using AspNetCoreEcommerce.Data;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreEcommerce.Repositories.Implementations
{
    public class ShippingAddressRespository(ApplicationDbContext context) : IShippingAddressRespository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<ShippingAddress?> AddShippingAddress(ShippingAddress shippingAddress)
        {
            try
            {
                await _context.ShippingAddresses.AddAsync(shippingAddress);
                await _context.SaveChangesAsync();
                return shippingAddress;
            }
            catch(Exception)
            {
                //throw new DbUpdateException("Error adding shipping address", ex)
                // log exception
                return null;
            }
        }

        public async Task<string?> DeleteShippingAddress(Guid customerId, Guid shippingid)
        {
            var shippingAddress = await _context.ShippingAddresses
                .Where(sh => sh.ShippingAddressId == shippingid && sh.CustomerId == customerId)
                .FirstOrDefaultAsync();
            if (shippingAddress == null)
                return null;

            _context.ShippingAddresses.Remove(shippingAddress);
            await _context.SaveChangesAsync();
            return "Shipping address removed";
        }

        public async Task<IEnumerable<ShippingAddress>> GetShippingAddressByCustomerId(Guid customerId)
        {
            var sh = await _context.ShippingAddresses
                .Where(sh => sh.CustomerId == customerId)
                .ToListAsync();

            return sh.Count == 0 ? [] : sh;
        }

        public async Task<ShippingAddress?> GetShippingAddressByIdAsync(Guid customerId, Guid shippingAddId)
        {
            var sh = await _context.ShippingAddresses
                .Where(sh => sh.CustomerId == customerId && sh.ShippingAddressId == shippingAddId)
                .FirstOrDefaultAsync();
            return sh is null ? null: sh;
        }

        public async Task<Customer?> GetCustomerByIdAsync(Guid customerId)
        {
            var cs = await _context.Customers.FindAsync(customerId);
            return cs is null ? null : cs;
        }
    }
}
