using Data;
using Entities;

using Microsoft.AspNetCore.Http;

namespace Repositories
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

        public async Task<Vendor> GetVendorByIdAsync(int vendorId)
        {
            var vendor = await _context.Vendors.FindAsync(vendorId) ?? throw new KeyNotFoundException($"Vendor with the Id {vendorId} was not found");
            return vendor;
        }

        public async Task<Vendor> UpdateVendorAsync(int vendorId, Vendor vendor)
        {
            var existingVendor = await _context.Vendors.FindAsync(vendorId) ?? throw new KeyNotFoundException($"Vendor with the Id {vendorId} was not found");
            existingVendor.DateUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return existingVendor;
        }

        public async Task DeleteVendorAsync(int vendorId)
        {
             var vendor = await _context.Vendors.FindAsync(vendorId) ?? throw new KeyNotFoundException($"Vendor with the Id {vendorId} was not found");
             vendor.IsDeleted = true;
             await _context.SaveChangesAsync();
        }

        public async Task<string?> SaveProductImageAsync(IFormFile imageFile, HttpRequest request)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Products");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = $"{Guid.NewGuid()}_{Path.GetExtension(imageFile.FileName)}".Replace(" ", "");
            var filePath = Path.Combine(uploadPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            var imageUrl = $"{request.Scheme}://{request.Host}/Uploads/Posts/{fileName}";
            return imageUrl;
        }

    }
}
