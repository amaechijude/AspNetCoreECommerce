using AspNetCoreEcommerce.Authentication;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Repositories.Contracts;
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
                    ? await GlobalConstants.SaveImageAsync(vendorDto1.VendorBanner, GlobalConstants.vendorSubPath)
                    : null;

            var newVendor = new Vendor
            {
                VendorId = Guid.CreateVersion7(),
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

            return MapVendorToDto(createVendor, request);
            
        }
        public async Task<VendorViewDto> GetVendorByIdAsync(Guid vendorId, HttpRequest request)
        {
            var vendor = await _vendorRepository.GetVendorByIdAsync(vendorId)
                ?? throw new KeyNotFoundException("Invalid or Non Existing Vendor");

            return MapVendorToDto(vendor, request);
        }
        public async Task<VendorViewDto> UpdateVendorByIdAsync(Guid vendorId, UpdateVendorDto upvendor, HttpRequest request)
        {
            var existingVendor = await _vendorRepository.GetVendorByIdAsync(vendorId)
                ?? throw new KeyNotFoundException($"Vendor was not found");

            var burl = upvendor.VendorBanner == null
                ? null
                :await GlobalConstants.SaveImageAsync(upvendor.VendorBanner, GlobalConstants.vendorSubPath);
            
            existingVendor.UpdateVendor(
                burl,upvendor.VendorPhone, upvendor.Location,
                upvendor.GoogleMapUrl, upvendor.TwitterUrl,
                upvendor.InstagramUrl, upvendor.FacebookUrl
                );

            await _vendorRepository.SaveUpdateVendorAsync();

            return MapVendorToDto(existingVendor, request);
        }
        // public async Task DeleteVendorAsync(Guid vendorId)
        // {
        //     var vendor = await _vendorRepository.GetVendorByIdAsync(vendorId)
        //         ?? throw new KeyNotFoundException($"Vendor with the Id {vendorId} was not found");
        //     await _vendorRepository.DeleteVendorAsync(vendor);
        // }

        public async Task<VendorLoginViewDto> LoginVendorAsync(LoginDto login)
        {
            if (string.IsNullOrEmpty(login.Email))
                throw new ArgumentException("Email cannot be empty");
                
            var vendor = await _vendorRepository.GetVendorByEmailAsync(login.Email)
                ?? throw new KeyNotFoundException("Invalid vendor email");

#pragma warning disable CS8604 // Possible null reference argument.
            var verifyLogin = _passwordHasher.VerifyHashedPassword(vendor, vendor.PasswordHash, login.Password);

            if (verifyLogin == PasswordVerificationResult.Failed)
                throw new ArgumentException("Invalid Password");

            var token = _tokenProvider.CreateVendorToken(vendor);

            vendor.LastLoginDate = DateTimeOffset.UtcNow;
            await _vendorRepository.SaveUpdateVendorAsync();

            return new VendorLoginViewDto {
                VendorId = vendor.VendorId,
                VendorEmail = vendor.VendorEmail,
                Token = token
            };
        }

        private static VendorViewDto MapVendorToDto(Vendor vendor, HttpRequest request)
        {
            return new VendorViewDto
            {
                VendorId = vendor.VendorId,
                VendorName = vendor.VendorName,
                VendorEmail = vendor.VendorEmail,
                VendorPhone = vendor.VendorPhone,
                VendorBannerUrl = GlobalConstants.GetImagetUrl(request, vendor.VendorBanner),
                Location = vendor.Location,
                TwitterUrl = vendor.TwitterUrl,
                FacebookUrl = vendor.FacebookUrl,
                DateJoined = vendor.DateJoined,
                LastLoginDate = vendor.LastLoginDate,
                InstagramUrl = vendor.InstagramUrl,
                Products = [.. vendor.Products.Select(p => new ProductViewDto {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    ImageUrl = GlobalConstants.GetImagetUrl(request, p.ImageName),
                    Price = p.Price,
                    VendorId = p.VendorId,
                    VendorName = p.Vendor.VendorName
                })]
            };
        }
    
    }
}
