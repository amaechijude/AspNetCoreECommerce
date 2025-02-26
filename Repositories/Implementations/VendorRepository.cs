using AspNetCoreEcommerce.Data;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreEcommerce.Repositories.Implementations
{
    public class VendorRepository(ApplicationDbContext context) : IVendorRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Vendor> SignupVendorAsync(Vendor vendor)
        {
            var existingVendorEmail = await _context.Vendors
                .AnyAsync(v => v.VendorEmail == vendor.VendorEmail);
            if (existingVendorEmail)
                throw new DuplicateException("Email is already taken");

            var existingVendorName = await _context.Vendors
                .AnyAsync(v => v.VendorName == vendor.VendorName);
            if (existingVendorName)
                throw new DuplicateException("Vendor Name is already taken");

            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();

            return vendor;

        }
        public async Task<Vendor> GetVendorByIdAsync(Guid vendorId)
        {
            var vendor = await _context.Vendors
                .Include(v => v.Products)
                .FirstOrDefaultAsync(v => v.VendorId == vendorId)
                ?? throw new KeyNotFoundException("Vendor does not Exist");
            return vendor;
        }

        public async Task<Vendor> GetVendorByEmailAsync(string email)
        {
            var vendor = await _context.Vendors
                .FirstOrDefaultAsync(v => v.VendorEmail == email)
                ?? throw new KeyNotFoundException("Invalid Vendor Email");

            return vendor;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public class DuplicateException(string ErrorMessage) : Exception(ErrorMessage);
    }
}