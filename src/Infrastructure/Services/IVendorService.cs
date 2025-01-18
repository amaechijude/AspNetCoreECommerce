using DataTransferObjects;
using Microsoft.AspNetCore.Http;

namespace Services
{
    public interface IVendorService
    {
        Task<VendorViewDto> CreateVendorAsync(VendorDto vendorDto, HttpRequest request);
        Task<VendorViewDto> GetVendorByIdAsync(int vendorId);
        Task<VendorViewDto> UpdateVendorIdAsync(int vendorId, VendorDto vendor);
        Task DeleteVendorAsync(int vendorId);
    }
}
