using AspNetCoreEcommerce.Data;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Respositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreEcommerce.Respositories.Implementations
{
    public class VendorRepository(ApplicationDbContext context) : IVendorRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Vendor> CreateVendorAsync(Vendor vendor, HttpRequest request)
        {
            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();
            return vendor;
        }

        public async Task<Vendor> GetVendorByIdAsync(Guid vendorId)
        {
            var vendor = await _context.Vendors.FindAsync(vendorId)
                ?? throw new ArgumentException("Inavlid User Id");
            return vendor;
        }

        public async Task SaveUpdateVendorAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVendorAsync(Vendor vendor)
        {
             vendor.IsDeleted = true;
             await _context.SaveChangesAsync();
        }

        public async Task<string?> SaveVendorBannerAsync(IFormFile imageFile, HttpRequest request)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            var subPath = GlobalConstants.vendorSubPath;
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), GlobalConstants.uploadPath, subPath);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = $"{Guid.NewGuid()}_{Path.GetExtension(imageFile.FileName)}".Replace(" ", "");
            var filePath = Path.Combine(uploadPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            return $"{GlobalConstants.uploadPath}/{subPath}/{fileName}";
        }

        public async Task<Vendor?> GetVendorByEmailAsync(string vendorEmail)
        {
            var vendor = await _context.Vendors.SingleOrDefaultAsync(v => v.VendorEmail == vendorEmail);
            return vendor ?? null;
        }

    }
}
