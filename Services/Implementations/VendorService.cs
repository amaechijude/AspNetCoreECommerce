using System.Threading.Channels;
using AspNetCoreEcommerce.Authentication;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.EmailService;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Repositories.Contracts;
using AspNetCoreEcommerce.ResultResponse;
using AspNetCoreEcommerce.Services.Contracts;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreEcommerce.Services.Implementations
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly TokenProvider _tokenProvider;
        private readonly Channel<EmailDto> _emailChannel;

        public VendorService(
            IVendorRepository vendorRepository,
            TokenProvider tokenProvider,
            Channel<EmailDto> emailChannel
            )
        {
            _vendorRepository = vendorRepository;
            _tokenProvider = tokenProvider;
            _emailChannel = emailChannel;
        }

        private readonly PasswordHasher<Vendor> _passwordHasher = new();

        public async Task<ResultPattern> SignupVendorAsync(VendorDto vendorDto, HttpRequest request)
        {
            if (string.IsNullOrWhiteSpace(vendorDto.VendorEmail))
                return ResultPattern.FailResult("Email is Required");
            if (string.IsNullOrWhiteSpace(vendorDto.Password))
                return ResultPattern.FailResult("Password is Required");
            if (string.IsNullOrWhiteSpace(vendorDto.VendorName))
                return ResultPattern.FailResult("Vendor Name is Required");
            if (string.IsNullOrWhiteSpace(vendorDto.Location))
                return ResultPattern.FailResult("Vendor Location is Required");

            if (await _vendorRepository.CheckExistingVendorEmailAsync(vendorDto.VendorEmail))
                return ResultPattern.FailResult($"Vendor with email:-> {vendorDto.VendorEmail} already exists");

            if (await _vendorRepository.CheckExistingVendorNameAsync(vendorDto.VendorName))
                return ResultPattern.FailResult($"Vendor with Name:-> {vendorDto.VendorName} already exists");

            var bannerurl = vendorDto.VendorBanner is null
                ? null
                : await GlobalConstants.SaveImageAsync(vendorDto.VendorBanner, $"{GlobalConstants.vendorSubPath}");

            var nVendor = PrepareVendorSignUp(vendorDto);
            nVendor.VendorBanner = bannerurl;
            nVendor.PasswordHash = _passwordHasher.HashPassword(nVendor, vendorDto.Password);
            var vericicationCode = GlobalConstants.GenerateVerificationCode();
            nVendor.VerificationCode = vericicationCode;
            await _vendorRepository.SignupVendorAsync(nVendor);
            var Emaildata = new EmailDto
            {
                EmailTo = nVendor.VendorEmail,
                Name = nVendor.VendorName,
                Subject = "Welcome to our store",
                Body = "Welcome to our store, we are glad to have you as a vendor \n" +
                     "Please use the code below to verify your account\n" +
                     $"{vericicationCode}"
            };
            await _emailChannel.Writer.WriteAsync(Emaildata);
            return ResultPattern.SuccessResult(MapToVendorViewDto(nVendor, request), "Vendor Created Successfully");
        }

        public async Task<ResultPattern> ActivateVendorAsync(string email, string code, HttpRequest request)
        {
            var vendor = await _vendorRepository.GetVendorByEmailAsync(email);
            if (vendor is null)
                return ResultPattern.FailResult("Vendor not found");
            if (vendor.VerificationCode != code)
                return ResultPattern.FailResult("Invalid Verification Code");
            vendor.IsActive = true;
            await _vendorRepository.SaveChangesAsync();
            return ResultPattern.SuccessResult(MapToVendorViewDto(vendor, request), "Vendor Activated Successfully");
        }

        public async Task<ResultPattern> GetVendorByIdAsync(Guid vendorId, HttpRequest request)
        {
            var vendor = await _vendorRepository.GetVendorByIdAsync(vendorId);
            return vendor is null
                ? ResultPattern.FailResult("problem getting Vendor")
                : ResultPattern.SuccessResult(MapToVendorViewDto(vendor, request), "Vendor Found"); 
        }

        public async Task<ResultPattern> UpdateVendorAsync(Guid vendorId, UpdateVendorDto updateVendor, HttpRequest request)
        {
            var vendor = await _vendorRepository.GetVendorByIdAsync(vendorId);
            if (vendor is null)
                return ResultPattern.FailResult("Vendor not found");
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
            return ResultPattern.SuccessResult(MapToVendorViewDto(vendor, request), "Update successful");

        }

        public async Task<ResultPattern> LoginVendorAsync(LoginDto loginDto)
        {
            var vendor = await  _vendorRepository.GetVendorByEmailAsync(loginDto.Email);
            if (vendor is null)
                return ResultPattern.FailResult("Vendor not found");

#pragma warning disable CS8604 // Possible null reference argument.
            var verifypassword = _passwordHasher.VerifyHashedPassword(vendor, vendor.PasswordHash, loginDto.Password);
#pragma warning restore CS8604 // Possible null reference argument.

            if (verifypassword == PasswordVerificationResult.Failed && vendor.PasswordHash != loginDto.Password)
                throw new KeyNotFoundException("Invalid Password");
            
            var token = _tokenProvider.CreateVendorToken(vendor);
            vendor.LoginDateUpdate();
            await _vendorRepository.SaveChangesAsync();
            var data = new VendorLoginViewDto
            {
                VendorId = vendor.VendorId,
                VendorEmail = vendor.VendorEmail,
                LastloginDate = vendor.LastLoginDate,
                Token = token
            };
            return ResultPattern.SuccessResult(data, "Login Successful");
        }

        private static Vendor PrepareVendorSignUp(VendorDto vendorDto)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            return new Vendor
            {
                VendorId = Guid.CreateVersion7(),
                VendorName = vendorDto.VendorName,
                VendorEmail = vendorDto.VendorEmail,
                VendorPhone = vendorDto.VendorPhone,
                Location = vendorDto.Location,
                GoogleMapUrl = vendorDto.GoogleMapUrl,
                TwitterUrl = vendorDto.TwitterUrl,
                InstagramUrl = vendorDto.InstagramUrl,
                FacebookUrl = vendorDto.FacebookUrl,
                DateJoined = DateTimeOffset.UtcNow
            };
#pragma warning restore CS8601 // Possible null reference assignment.
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