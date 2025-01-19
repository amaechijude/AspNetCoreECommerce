using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Respositories.Contracts;
using AspNetCoreEcommerce.Services.Contracts;

namespace AspNetCoreEcommerce.Services.Implementations
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

            var bannerUrl = vendorDto1.VendorBanner != null 
                    ? await _vendorRepository.SaveVendorBannerAsync(vendorDto1.VendorBanner, request)
                    : null;

            var newVendor = new Vendor
            {
                VendorName = vendorDto1.VendorName,
                VendorEmail = vendorDto1.VendorEmail,
                VendorPhone = vendorDto1.VendorPhone,
                VendorBannerUrl = bannerUrl,
                Location = vendorDto1.Location,
                GoogleMapUrl = vendorDto1.GoogleMapUrl,
                TwitterUrl = vendorDto1.TwitterUrl,
                FacebookUrl = vendorDto1.FacebookUrl,
                DateJoined = DateTime.UtcNow,
            };
            var createVendor = await _vendorRepository.CreateVendorAsync(newVendor, request);
            var vendorView = new VendorViewDto
            {
                VendorId = createVendor.VendorId,
                VendorName = createVendor.VendorName,
                VendorEmail = createVendor.VendorEmail,
                VendorPhone = createVendor.VendorPhone,
                VendorBannerUrl = createVendor.VendorBannerUrl,
                Location = createVendor.Location,
                TwitterUrl = createVendor.TwitterUrl,
                FacebookUrl = createVendor.FacebookUrl,
                DateJoined = createVendor.DateJoined,
                InstagramUrl = createVendor.InstagramUrl,
            };
            return vendorView;
            
        }
        public async Task<VendorViewDto?> GetVendorByIdAsync(int vendorId)
        {
            var vendor = await _vendorRepository.GetVendorByIdAsync(vendorId);
            if (vendor == null)
                return null;

            var vendorView = new VendorViewDto
            {
                VendorId = vendor.VendorId,
                VendorName = vendor.VendorName,
                VendorEmail = vendor.VendorEmail,
                VendorPhone = vendor.VendorPhone,
                VendorBannerUrl = vendor.VendorBannerUrl,
                Location = vendor.Location,
                TwitterUrl = vendor.TwitterUrl,
                FacebookUrl = vendor.FacebookUrl,
                DateJoined = vendor.DateJoined,
                InstagramUrl = vendor.InstagramUrl,
            };
            return vendorView;
        }
        public async Task<VendorViewDto> UpdateVendorByIdAsync(int vendorId, VendorDto upvendor, HttpRequest request)
        {
            var existingVendor = await _vendorRepository.GetVendorByIdAsync(vendorId) ?? throw new InvalidOperationException("");
            var bannerUrl = upvendor.VendorBanner != null
                    ? await _vendorRepository.SaveVendorBannerAsync(upvendor.VendorBanner, request)
                    : null;

            existingVendor.VendorName = upvendor.VendorName;
            existingVendor.VendorEmail = upvendor.VendorEmail;
            existingVendor.VendorPhone = upvendor.VendorPhone;
            existingVendor.VendorBannerUrl = bannerUrl;
            existingVendor.TwitterUrl = upvendor.TwitterUrl;
            existingVendor.FacebookUrl = upvendor.FacebookUrl;
            existingVendor.InstagramUrl = upvendor.InstagramUrl;
            existingVendor.DateUpdated = DateTime.UtcNow;

            var vendor = await _vendorRepository.UpdateVendorAsync(vendorId, existingVendor);
            var vendorView = new VendorViewDto
            {
                VendorId = vendor.VendorId,
                VendorName = vendor.VendorName,
                VendorEmail = vendor.VendorEmail,
                VendorPhone = vendor.VendorPhone,
                VendorBannerUrl = vendor.VendorBannerUrl,
                Location = vendor.Location,
                TwitterUrl = vendor.TwitterUrl,
                FacebookUrl = vendor.FacebookUrl,
                DateJoined = vendor.DateJoined,
                InstagramUrl = vendor.InstagramUrl,
            };
            return vendorView;
        }
        public async Task DeleteVendorAsync(int vendorId)
        {
            await _vendorRepository.DeleteVendorAsync(vendorId);
        }
    }
}