using Repositories;
using DataTransferObjects;
using Entities;

namespace Services
{
    public class VendorService(IVendorRepository repository) : IVendorService
    {
        private readonly IVendorRepository _vendorRepository = repository;
        public async Task<VendorViewDto> CreateVendorAsync(VendorDto vendorDto1)
        {   
            if (string.IsNullOrWhiteSpace(vendorDto1.VendorName) || string.IsNullOrWhiteSpace(vendorDto1.VendorEmail))
                throw new Exception("Vendor name and Email cannot be empty");

            if (string.IsNullOrWhiteSpace(vendorDto1.VendorPhone) || string.IsNullOrWhiteSpace(vendorDto1.Location))
                throw new Exception("Vendor Phone and location cannot be empty");

            if (vendorDto1.VendorBanner != null)
                var imageUrl = 
        }
        Task<VendorViewDto> GetVendorByIdAsync(int vendorId);
        Task<VendorViewDto> UpdateVendorIdAsync(int vendorId, VendorDto vendor);
        Task DeleteVendorAsync(int vendorId);
    }
}