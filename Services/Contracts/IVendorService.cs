using AspNetCoreEcommerce.DTOs;

namespace AspNetCoreEcommerce.Services.Contracts
{
    public interface IVendorService
    {
        Task<VendorViewDto> CreateVendorAsync(VendorDto vendorDto, HttpRequest request);
        Task<VendorViewDto> GetVendorByIdAsync(Guid vendorId, HttpRequest request);
        Task<VendorViewDto> UpdateVendorByIdAsync(Guid vendorId, UpdateVendorDto upvendor, HttpRequest request);
        // Task DeleteVendorAsync(Guid vendorId);
        Task<VendorLoginViewDto> LoginVendorAsync(LoginDto login);
    }
}
