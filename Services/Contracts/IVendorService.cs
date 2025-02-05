using AspNetCoreEcommerce.DTOs;

namespace AspNetCoreEcommerce.Services.Contracts
{
    public interface IVendorService
    {
        Task<VendorViewDto> CreateVendorAsync(VendorDto vendorDto, HttpRequest request);
        Task<VendorViewDto?> GetVendorByIdAsync(int vendorId);
        Task<VendorViewDto> UpdateVendorByIdAsync(int vendorId, VendorDto vendor, HttpRequest request);
        Task DeleteVendorAsync(int vendorId);
        Task<VendorLoginViewDto> LoginVendorAsync(LoginDto login);
    }
}
