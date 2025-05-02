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
            return await _context.Vendors.AnyAsync(
                v => v.UserId == userId
                || string.Equals(v.VendorEmail, email, StringComparison.OrdinalIgnoreCase)
                || string.Equals(v.VendorName, name, StringComparison.OrdinalIgnoreCase)
                );
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public Task<Vendor?> GetVendorByEmailAsync(string email)
        {
            return _context.Vendors.FirstOrDefaultAsync(v => v.VendorEmail == email);
        }

        public async Task<Vendor?> GetVendorByIdAsync(Guid veondorId)
        {
            return await _context.Vendors
                .Include(v => v.Products)
                .FirstOrDefaultAsync(v => v.VendorId == veondorId);
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