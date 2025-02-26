using AspNetCoreEcommerce.Data;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreEcommerce.Repositories.Implementations
{
    public class ShippingAddressRespository(ApplicationDbContext context) : IShippingAddressRespository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<ShippingAddress> AddShippingAddress(ShippingAddress shippingAddress)
        {
            await _context.ShippingAddresses.AddAsync(shippingAddress);
            await _context.SaveChangesAsync();
            return shippingAddress;
        }

        public async Task DeleteShippingAddress(Guid customerId, Guid shippingid)
        {
            var shippingAddress = await _context.ShippingAddresses
                .Where(sh => sh.ShippingAddressId == shippingid && sh.CustomerId == customerId)
                .FirstOrDefaultAsync()
                ?? throw new KeyNotFoundException("Shipping address deleted or does not exist");

            _context.ShippingAddresses.Remove(shippingAddress);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ShippingAddress>> GetShippingAddressByCustomerId(Guid customerId)
        {
            var customer = await GetCustomerByIdAsync(customerId);
            var sh = await _context.ShippingAddresses
                .Where(sh => sh.CustomerId == customer.CustomerID)
                .ToListAsync();

            return sh.Count > 0 ? sh : [];
        }

        public async Task<Customer> GetCustomerByIdAsync(Guid customerId)
        {
            return await _context.Customers.FindAsync(customerId)
                ?? throw new KeyNotFoundException("Invalid Customer");
        }
    }
}
