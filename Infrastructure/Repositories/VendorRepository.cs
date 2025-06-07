using AspNetCoreEcommerce.Application.Interfaces.Repositories;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreEcommerce.Infrastructure.Repositories
{
    public class VendorRepository(ApplicationDbContext context) : IVendorRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<bool> CheckUniqueNameEmail(Guid userId, string email, string name)
        {
            return await _context.Vendors.AnyAsync(v => 
            v.UserId == userId 
            || v.VendorEmail == email 
            || v.VendorName == name
            );
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<(IEnumerable<Product> Items, int TotalCount)> GetVendorPagedProductAsync(Guid vendorId, int pageNumber, int pageSize)
        {
            var query = _context.Products
                .AsQueryable()
                .Where(v => v.VendorId == vendorId);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public Task<Vendor?> GetVendorByEmailAsync(string email)
        {
            return _context.Vendors.FirstOrDefaultAsync(v => v.VendorEmail == email);
        }

        public async Task<Vendor?> GetVendorByIdAsync(Guid vendorId)
        {
            return await _context.Vendors.FindAsync(vendorId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void CreateVendor(Vendor vendor)
        {
            _context.Vendors.Add(vendor);
        }
    }
}