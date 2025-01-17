using DataTransferObjects;

namespace Services
{
    public interface IVendorService
    {
        Task<VendorViewDto> CreateVendorAsync(VendorDto vendorDto);
        Task<VendorViewDto> GetVendorByIdAsync(int vendorId);
        Task<VendorViewDto> UpdateVendorIdAsync(int vendorId, VendorDto vendor);
        Task DeleteVendorAsync(int vendorId);
    }
}
