using AspNetCoreEcommerce.Authentication;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Repositories.Contracts;
using AspNetCoreEcommerce.Services.Contracts;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreEcommerce.Services.Implementations
{
    public class VendorService(IVendorRepository vendorRepository, TokenProvider tokenProvider) : IVendorService
    {
        private readonly IVendorRepository _vendorRepository = vendorRepository;
        private readonly PasswordHasher<Vendor> _passwordHasher = new();
        private readonly TokenProvider _tokenProvider = tokenProvider;

        public async Task<VendorViewDto> SignupVendorAsync(VendorDto vendorDto, HttpRequest request)
        {
            if (string.IsNullOrWhiteSpace(vendorDto.VendorEmail) || string.IsNullOrWhiteSpace(vendorDto.Password))
                throw new ArgumentException("Email and Password is Required");
                
            if (string.IsNullOrWhiteSpace(vendorDto.VendorName))
                throw new ArgumentException("Vendor Name is Required");

            if (string.IsNullOrWhiteSpace(vendorDto.Location))
                throw new ArgumentException("Vendor Location is Required");

            var bannerurl = vendorDto.VendorBanner is null
                ? null
                : await GlobalConstants.SaveImageAsync(vendorDto.VendorBanner, $"{GlobalConstants.vendorSubPath}");

            var nVendor = new Vendor
            {
                VendorId = Guid.CreateVersion7(),
                VendorName = vendorDto.VendorName,
                VendorEmail = vendorDto.VendorEmail,
                VendorPhone = vendorDto.VendorPhone,
                VendorBanner = bannerurl,
                Location = vendorDto.Location,
                GoogleMapUrl = vendorDto.GoogleMapUrl,
                TwitterUrl = vendorDto.TwitterUrl,
                InstagramUrl = vendorDto.InstagramUrl,
                FacebookUrl = vendorDto.FacebookUrl,
                DateJoined = DateTimeOffset.UtcNow
            };
            nVendor.PasswordHash = _passwordHasher.HashPassword(nVendor, vendorDto.Password);
            await _vendorRepository.SignupVendorAsync(nVendor);
            
            return MapToVendorViewDto(nVendor, request);
        }

        public async Task<VendorViewDto> GetVendorByIdAsync(Guid vendorId, HttpRequest request)
        {
            var vendor = await _vendorRepository.GetVendorByIdAsync(vendorId);
            return MapToVendorViewDto(vendor, request);
        }

        public async Task<VendorViewDto> UpdateVendorAsync(Guid vendorId, UpdateVendorDto updateVendor, HttpRequest request)
        {
            var vendor = await _vendorRepository.GetVendorByIdAsync(vendorId);
            var bannerurl = updateVendor.VendorBanner is null
                ? null
                : await GlobalConstants.SaveImageAsync(updateVendor.VendorBanner, $"{GlobalConstants.vendorSubPath}");

            vendor.UpdateVendor(
                bannerurl, updateVendor.VendorPhone,
                updateVendor.Location, updateVendor.GoogleMapUrl,
                updateVendor.TwitterUrl,
                updateVendor.InstagramUrl, updateVendor.FacebookUrl
                );

            await _vendorRepository.SaveChangesAsync();
            return MapToVendorViewDto(vendor, request);
        }

        public async Task<VendorLoginViewDto> LoginVendorAsync(LoginDto loginDto)
        {
            var vendor = await  _vendorRepository.GetVendorByEmailAsync(loginDto.Email);

#pragma warning disable CS8604 // Possible null reference argument.
            var verifypassword = _passwordHasher.VerifyHashedPassword(vendor, vendor.PasswordHash, loginDto.Password);
#pragma warning restore CS8604 // Possible null reference argument.

            if (verifypassword == PasswordVerificationResult.Failed && vendor.PasswordHash != loginDto.Password)
                throw new KeyNotFoundException("Invalid Password");
            
            var token = _tokenProvider.CreateVendorToken(vendor);
            vendor.LoginDateUpdate();
            await _vendorRepository.SaveChangesAsync();
            return new VendorLoginViewDto
            {
                VendorId = vendor.VendorId,
                VendorEmail = vendor.VendorEmail,
                LastloginDate = vendor.LastLoginDate,
                Token = token
            };
        }

        private static VendorViewDto MapToVendorViewDto(Vendor vendor, HttpRequest request)
        {
             return new VendorViewDto
            {
                VendorId = vendor.VendorId,
                VendorName = vendor.VendorName,
                VendorEmail = vendor.VendorEmail,
                VendorPhone = vendor.VendorPhone,
                VendorBannerUrl = GlobalConstants.GetImagetUrl(request, vendor.VendorBanner),
                Location = vendor.Location,
                GoogleMapUrl = vendor.GoogleMapUrl,
                TwitterUrl = vendor.TwitterUrl,
                InstagramUrl = vendor.InstagramUrl,
                FacebookUrl = vendor.FacebookUrl,
                DateJoined = vendor.DateJoined,
                DateUpdated = vendor.DateUpdated,
                LastLoginDate = vendor.LastLoginDate,
                Products = [.. vendor.Products.Select(v => new ProductViewDto {
                    ProductId = v.ProductId,
                    ProductName = v.ProductName,
                    Description = v.Description,
                    ImageUrl = GlobalConstants.GetImagetUrl(request, v.ImageUrl),
                    Price = v.Price,
                    VendorId = v.VendorId,
                    VendorName = v.VendorName
                })]
            };
        }  
    }
}