using AspNetCoreEcommerce.DTOs;

namespace AspNetCoreEcommerce.Services.Contracts
{
    public interface IVendorService
    {
        Task<VendorViewDto> SignupVendorAsync(VendorDto vendorDto, HttpRequest request);
        Task<VendorViewDto> UpdateVendorAsync(Guid vendorId, UpdateVendorDto updateVendor, HttpRequest request);
        Task<VendorViewDto> GetVendorByIdAsync(Guid vendorId, HttpRequest request);
        Task<VendorLoginViewDto> LoginVendorAsync(LoginDto loginDto);
    }
}