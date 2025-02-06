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
                VendorBanner = bannerUrl,
                Location = vendorDto1.Location,
                GoogleMapUrl = vendorDto1.GoogleMapUrl,
                TwitterUrl = vendorDto1.TwitterUrl,
                FacebookUrl = vendorDto1.FacebookUrl,
                DateJoined = DateTimeOffset.UtcNow,
            };
            newVendor.PasswordHash = _passwordHasher.HashPassword(newVendor, vendorDto1.Password);

            var createVendor = await _vendorRepository.CreateVendorAsync(newVendor, request);

            return new VendorViewDto
            {
                VendorId = createVendor.VendorId,
                VendorName = createVendor.VendorName,
                VendorEmail = createVendor.VendorEmail,
                VendorPhone = createVendor.VendorPhone,
                VendorBannerUrl = GeImagetUrl(request, createVendor.VendorBanner),
                Location = createVendor.Location,
                TwitterUrl = createVendor.TwitterUrl,
                FacebookUrl = createVendor.FacebookUrl,
                DateJoined = createVendor.DateJoined,
                InstagramUrl = createVendor.InstagramUrl,
            };
            
        }
        public async Task<VendorViewDto> GetVendorByIdAsync(Guid vendorId, HttpRequest request)
        {
            var vendor = await _vendorRepository.GetVendorByIdAsync(vendorId)
                ?? throw new KeyNotFoundException("Invalid or Non Existing Vendor");

            var vendorView = new VendorViewDto
            {
                VendorId = vendor.VendorId,
                VendorName = vendor.VendorName,
                VendorEmail = vendor.VendorEmail,
                VendorPhone = vendor.VendorPhone,
                VendorBannerUrl = GeImagetUrl(request, vendor.VendorBanner),
                Location = vendor.Location,
                TwitterUrl = vendor.TwitterUrl,
                FacebookUrl = vendor.FacebookUrl,
                DateJoined = vendor.DateJoined,
                InstagramUrl = vendor.InstagramUrl,
            };
            return vendorView;
        }
        public async Task<VendorViewDto> UpdateVendorByIdAsync(Guid vendorId, UpdateVendorDto upvendor, HttpRequest request)
        {
            var existingVendor = await _vendorRepository.GetVendorByIdAsync(vendorId)
                ?? throw new KeyNotFoundException($"Vendor was not found");

            var bannerUrl = upvendor.VendorBanner != null
                    ? await _vendorRepository.SaveVendorBannerAsync(upvendor.VendorBanner, request)
                    : null;
            
            existingVendor.UpdateVendor(
                upvendor.VendorPhone, upvendor.Location, upvendor.GoogleMapUrl,
                upvendor.TwitterUrl, upvendor.InstagramUrl, upvendor.FacebookUrl
                );

            await _vendorRepository.SaveUpdateVendorAsync();

            var vendor = existingVendor;
            var vendorView = new VendorViewDto
            {
                VendorId = vendor.VendorId,
                VendorName = vendor.VendorName,
                VendorEmail = vendor.VendorEmail,
                VendorPhone = vendor.VendorPhone,
                VendorBannerUrl = GeImagetUrl(request, vendor.VendorBanner),
                Location = vendor.Location,
                TwitterUrl = vendor.TwitterUrl,
                FacebookUrl = vendor.FacebookUrl,
                DateJoined = vendor.DateJoined,
                InstagramUrl = vendor.InstagramUrl,
            };
            return vendorView;
        }
        public async Task DeleteVendorAsync(Guid vendorId)
        {
            var vendor = await _vendorRepository.GetVendorByIdAsync(vendorId)
                ?? throw new KeyNotFoundException($"Vendor with the Id {vendorId} was not found");
            await _vendorRepository.DeleteVendorAsync(vendor);
        }

        public async Task<VendorLoginViewDto> LoginVendorAsync(LoginDto login)
        {
            var vendor = await _vendorRepository.GetVendorByEmailAsync(login.Email)
                ?? throw new KeyNotFoundException("Invalid vendor email");

#pragma warning disable CS8604 // Possible null reference argument.
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

        private static string GeImagetUrl(HttpRequest request, string? imgUrl)
        {
            if (string.IsNullOrWhiteSpace(imgUrl))
                return "";
            return $"{request.Scheme}://{request.Host}/{GlobalConstants.uploadPath}/{imgUrl}";
        }
    }
}
