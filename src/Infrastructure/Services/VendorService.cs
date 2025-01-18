using Repositories;
using DataTransferObjects;
using Entities;
using Microsoft.AspNetCore.Http;

namespace Services
{
    public class VendorService(IVendorRepository repository) : IVendorService
    {
        private readonly IVendorRepository _vendorRepository = repository;
        public async Task<VendorViewDto> CreateVendorAsync(VendorDto vendorDto1, HttpRequest request)
        {   
            if (string.IsNullOrWhiteSpace(vendorDto1.VendorName) || string.IsNullOrWhiteSpace(vendorDto1.VendorEmail))
                throw new Exception("Vendor name and Email cannot be empty");

            if (string.IsNullOrWhiteSpace(vendorDto1.VendorPhone) || string.IsNullOrWhiteSpace(vendorDto1.Location))
                throw new Exception("Vendor Phone and location cannot be empty");

            var newVendor = new Vendor
            {
                VendorName = vendorDto1.VendorName,
                VendorEmail = vendorDto1.VendorEmail,
                VendorPhone = vendorDto1.VendorPhone,
                Location = vendorDto1.Location,
                GoogleMapUrl = vendorDto1.GoogleMapUrl,
                TwitterUrl = vendorDto1.TwitterUrl,
                FacebookUrl = vendorDto1.FacebookUrl,
                VendorBannerUrl = vendorDto1.VendorBanner != null 
                    ? await _vendorRepository.SaveVendorBannerAsync(vendorDto1.VendorBanner, request)
                    : null
            };
            
        }
        Task<VendorViewDto> GetVendorByIdAsync(int vendorId);
        Task<VendorViewDto> UpdateVendorIdAsync(int vendorId, VendorDto vendor);
        Task DeleteVendorAsync(int vendorId);
    }
}