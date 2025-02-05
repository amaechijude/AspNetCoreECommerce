using AspNetCoreEcommerce.Authentication;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Respositories.Contracts;
using AspNetCoreEcommerce.Services.Contracts;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreEcommerce.Services.Implementations
{
    public class VendorService(IVendorRepository repository, TokenProvider tokenProvider) : IVendorService
    {
        private readonly IVendorRepository _vendorRepository = repository;
        private readonly TokenProvider _tokenProvider = tokenProvider;
        private readonly PasswordHasher<Vendor> _passwordHasher = new();

        public async Task<VendorViewDto> CreateVendorAsync(VendorDto vendorDto1, HttpRequest request)
        {   
            if (string.IsNullOrWhiteSpace(vendorDto1.VendorName))
                throw new ArgumentException("Vendor Name cannot be empty");

            if (string.IsNullOrWhiteSpace(vendorDto1.VendorEmail) || string.IsNullOrEmpty(vendorDto1.Password))
                throw new ArgumentException("Email and Password Cannont be empty");

            if (string.IsNullOrWhiteSpace(vendorDto1.VendorPhone) || string.IsNullOrWhiteSpace(vendorDto1.Location))
                throw new ArgumentException("Vendor Phone and location cannot be empty");

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
                DateJoined = DateTimeOffset.UtcNow,
            };
            newVendor.PasswordHash = _passwordHasher.HashPassword(newVendor, vendorDto1.Password);

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
            var existingVendor = await _vendorRepository.GetVendorByIdAsync(vendorId)
                ?? throw new KeyNotFoundException($"Vendor with the Id {vendorId} was not found");

            var bannerUrl = upvendor.VendorBanner != null
                    ? await _vendorRepository.SaveVendorBannerAsync(upvendor.VendorBanner, request)
                    : null;
            
            existingVendor.UpdateVendor(upvendor.VendorPhone, upvendor.Location, upvendor.GoogleMapUrl, upvendor.TwitterUrl, upvendor.InstagramUrl, upvendor.FacebookUrl);
            await _vendorRepository.SaveUpdateVendorAsync();

            var vendor = existingVendor;
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
            var vendor = await _vendorRepository.GetVendorByIdAsync(vendorId)
                ?? throw new KeyNotFoundException($"Vendor with the Id {vendorId} was not found");
            await _vendorRepository.DeleteVendorAsync(vendor);
        }

        public async Task<VendorLoginViewDto> LoginVendorAsync(LoginDto login)
        {
            var vendor = await _vendorRepository.GetVendorByEmailAsync(login.Email);
            var verifyLogin = _passwordHasher.VerifyHashedPassword(vendor, vendor.PasswordHash, login.Password);

            if (verifyLogin == PasswordVerificationResult.Failed)
                throw new ArgumentException("Invalid Password");

            var token = _tokenProvider.Create(vendor);

            return new VendorLoginViewDto {
                VendorId = vendor.VendorId,
                VendorEmail = vendor.VendorEmail,
                Token = token
            };
        }
    }
}
