using Entities;
using Microsoft.AspNetCore.Http;

namespace Repositories
{
    public interface IVendorRepository
    {
        Task<Vendor> CreateVendorAsync(Vendor vendor);
        Task<Vendor> GetVendorByIdAsync(int vendorId);
        Task<Vendor> UpdateVendorAsync(int vendorId, Vendor vendor);
        Task DeleteVendorAsync(int vendorId);
        Task<string?> SaveVendorBannerAsync(IFormFile imageFile, HttpRequest request);
    }
}
