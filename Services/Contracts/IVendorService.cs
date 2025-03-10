using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.ResultResponse;

namespace AspNetCoreEcommerce.Services.Contracts
{
    public interface IVendorService
    {
        Task<ResultPattern> SignupVendorAsync(VendorDto vendorDto, HttpRequest request);
        Task<ResultPattern> UpdateVendorAsync(Guid vendorId, UpdateVendorDto updateVendor, HttpRequest request);
        Task<ResultPattern> GetVendorByIdAsync(Guid vendorId, HttpRequest request);
        Task<ResultPattern> LoginVendorAsync(LoginDto loginDto);
    }
}
