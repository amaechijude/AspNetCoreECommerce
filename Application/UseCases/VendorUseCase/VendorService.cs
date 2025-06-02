using System.Threading.Channels;
using AspNetCoreEcommerce.Application.Interfaces.Repositories;
using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Application.UseCases.ProductUseCase;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Infrastructure.EmailInfrastructure;
using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Application.UseCases.VendorUseCase
{
    public class VendorService(
        IVendorRepository vendorRepository,
        Channel<EmailDto> emailChannel
            ) : IVendorService
    {
        private readonly IVendorRepository _vendorRepository = vendorRepository;
        private readonly Channel<EmailDto> _emailChannel = emailChannel;

        public async Task<ResultPattern> CreateVendorAsync(User user, CreateVendorDto createVerndor, HttpRequest request)
        {
            var validator = new CreateVendorValidator();
            var result = await validator.ValidateAsync(createVerndor);
            if (!result.IsValid)
                return ResultPattern.FailResult(result.Errors);

            if (await _vendorRepository.CheckUniqueNameEmail(user.Id, createVerndor.Email, createVerndor.Name))
                return ResultPattern.FailResult("Creation failed: Possible duplicate request");

            var vendor = PrepareVendorSignUp(user, createVerndor);
            vendor.VendorBannerUri = await GlobalConstants.SaveImageAsync(createVerndor.Logo, GlobalConstants.vendorSubPath);
            vendor.VerificationCode = GlobalConstants.GenerateVerificationCode();
            _vendorRepository.CreateVendor(vendor);
            user.Vendor = vendor;
            user.VendorId = vendor.VendorId;
            user.IsVendor = true;
            await _vendorRepository.SaveChangesAsync();
            await _emailChannel
                .Writer.WriteAsync(new EmailDto
                {
                    Name = vendor.VendorName,
                    EmailTo = vendor.VendorEmail,
                    Subject = "Vendor Account Created",
                    Body = $"Hello {vendor.VendorName}, your account has been created successfully." +
                    $"Your verification code is {vendor.VerificationCode}"
                });
            return ResultPattern.SuccessResult("Successful");
        }

        public async Task<ResultPattern> ActivateVendorAsync( ActivateVendorDto dto,HttpRequest request)
        {
            var vendor = await _vendorRepository.GetVendorByEmailAsync(dto.Email);
            if (vendor is null || vendor.VerificationCode != dto.Code)
                return ResultPattern.FailResult("Verification Failed");

            vendor.ActivateVendor();
            await _vendorRepository.SaveChangesAsync();
            return ResultPattern.SuccessResult("Vendor account activated successfully");
        }

        public async Task<ResultPattern> UpdateVendorAsync(Guid vendorId, UpdateVendorDto updateVendor, HttpRequest request)
        {
            var vendor = await _vendorRepository.GetVendorByIdAsync(vendorId);
            if (vendor is null)
                return ResultPattern.FailResult("Update failed");
            await vendor.UpdateVendor(updateVendor);
            await _vendorRepository.SaveChangesAsync();
            return ResultPattern.SuccessResult("Vendor updated successfully");
        }

        public async Task<ResultPattern> GetVendorByIdAsync(User user, HttpRequest request)
        {
            if (!user.IsVendor)
                return ResultPattern.FailResult("User is not a vendor");
            var vendor = await _vendorRepository.GetVendorByIdAsync(user.VendorId);
            if (vendor is null)
                return ResultPattern.FailResult("Vendor Information not found");
            if (!vendor.IsActivated)
                return ResultPattern.FailResult("Vendor account is not activated yet");
            var vendorView = MapToVendorViewDto(vendor, request);
            return ResultPattern.SuccessResult(vendorView);
        }

        private static Vendor PrepareVendorSignUp(User user, CreateVendorDto vendorDto)
        {
            return new Vendor
            {
                User = user,
                UserId = user.Id,
                VendorId = Guid.CreateVersion7(),
                VendorName = vendorDto.Name,
                VendorEmail = vendorDto.Email,
                VendorPhone = vendorDto.Phone,
                Location = vendorDto.Location,
                DateJoined = DateTimeOffset.UtcNow
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
                BannerUrl = GlobalConstants.GetImagetUrl(request,vendor.VendorBannerUri),
                Location = vendor.Location,
                GoogleMapUrl = vendor.GoogleMapUrl,
                TwitterUrl = vendor.TwitterUrl,
                InstagramUrl = vendor.InstagramUrl,
                FacebookUrl = vendor.FacebookUrl,
                DateJoined = vendor.DateJoined,
                Products = [.. vendor.Products.Select(v => new ProductViewDto {
                    ProductId = v.ProductId,
                    ProductName = v.Name,
                    Description = v.Description,
                    ImageUrl = GlobalConstants.GetImagetUrl(request, v.ImageUrl),
                    Price = v.Price,
                    VendorId = v.VendorId,
                    VendorName = v.VendorName,
                    Stock = v.StockQuantity
                })]
            };
        }
    }
}