using AspNetCoreEcommerce.Data;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using static AspNetCoreEcommerce.Repositories.Implementations.CustomerRepository;

namespace AspNetCoreEcommerce.Repositories.Implementations
{

    public class VendorRepository(ApplicationDbContext context) : IVendorRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Vendor> CreateVendorAsync(Vendor vendor)
        {
            var vendorExists = await _context.Vendors
                .AnyAsync(v => v.VendorEmail == vendor.VendorEmail);

            if (vendorExists)
                throw new DuplicateEmailException("Email already exists");
                
            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();
            return vendor;
        }

        public async Task<Vendor> GetVendorByIdAsync(Guid vendorId)
        {
            var vendor = await _context.Vendors
                .Include(p => p.Products)
                .FirstOrDefaultAsync(v => v.VendorId == vendorId)
                ?? throw new ArgumentException("Inavlid User Id");
            return vendor;
        }

        public async Task SaveUpdateVendorAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Vendor> GetVendorByEmailAsync(string vendorEmail)
        {
            var vendor = await _context.Vendors
                .FirstOrDefaultAsync(v => v.VendorEmail == vendorEmail)
                ?? throw new KeyNotFoundException("Vendor not found");

            return vendor;
        }

    }
}
