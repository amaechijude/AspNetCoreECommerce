using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.Respositories.Contracts
{
    public interface IVendorRepository
    {
        Task<Vendor> CreateVendorAsync(Vendor vendor, HttpRequest request);
        Task<Vendor?> GetVendorByIdAsync(int vendorId);
        Task<Vendor> UpdateVendorAsync(int vendorId, Vendor vendor);
        Task DeleteVendorAsync(int vendorId);
        Task<string?> SaveVendorBannerAsync(IFormFile imageFile, HttpRequest request);
    }
}
