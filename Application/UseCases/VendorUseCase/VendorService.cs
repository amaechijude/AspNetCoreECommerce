using System.Threading.Channels;
using AspNetCoreEcommerce.Application.Interfaces.Repositories;
using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Application.UseCases.ProductUseCase;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Infrastructure.EmailInfrastructure;
using AspNetCoreEcommerce.Shared;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreEcommerce.Application.UseCases.VendorUseCase
{
    public class VendorService(
        IVendorRepository vendorRepository,
        Channel<EmailDto> emailChannel,
        UserManager<User> userManager
            ) : IVendorService
    {
        private readonly IVendorRepository _vendorRepository = vendorRepository;
        private readonly Channel<EmailDto> _emailChannel = emailChannel;
        private readonly UserManager<User> _userManager = userManager;
        private const int MaxPageSize = 50;

        public async Task<ResultPattern> CreateVendorAsync(User user, CreateVendorDto dto, HttpRequest request)
        {
            var validator = new CreateVendorValidator();
            var result = await validator.ValidateAsync(dto);
            if (!result.IsValid || string.IsNullOrEmpty(user.Email))
                return ResultPattern.FailResult(result.Errors);

            if (await _vendorRepository.CheckUniqueNameEmail(user.Id, dto.Email, dto.Name))
                return ResultPattern.FailResult("Creation failed: Possible duplicate request");

            var vendor = PrepareVendorSignUp(user, dto);
            if (dto.Logo is not null)
            {
                vendor.VendorLogoUri = await GlobalConstants
                    .SaveImageAsync(dto.Logo, GlobalConstants.vendorSubPath);
            }
            if (dto.Banner is not null)
            {

                vendor.VendorBannerUri = await GlobalConstants
                    .SaveImageAsync(dto.Banner, GlobalConstants.vendorSubPath);
            }
            _vendorRepository.CreateVendor(vendor);
            user.Vendor = vendor;
            user.VendorId = vendor.VendorId;
            user.IsVendor = true;

            var codes = new VendorVerificationCode
            {
                Id = Guid.NewGuid(),
                Vendor = vendor,
                VendorId = vendor.VendorId
            };

            await _vendorRepository.SaveChangesAsync();
            await _emailChannel
                .Writer.WriteAsync(new EmailDto
                {
                    Name = vendor.VendorName,
                    EmailTo = vendor.VendorEmail,
                    Subject = "Verify Your Vendor Account",
                    Body = EmailBodyTemplates.
                    ConfirmVendorRegistration(codes.Code, vendor.VendorEmail, user.Email)
                });
            return ResultPattern.SuccessResult("Successful");
        }

        public async Task<ResultPattern> ActivateVendorAsync(ActivateVendorDto dto,HttpRequest request)
        {
            User? user = await _userManager.FindByEmailAsync(dto.UserEmail);
            if (user is null)
                return ResultPattern.FailResult("User does not exist");
            var vendor = await _vendorRepository.GetVendorByEmailAsync(dto.VendorEmail);
            if (vendor is null || user.VendorId != vendor.VendorId)
                return ResultPattern.FailResult("Vendor not found");
            if (vendor.IsActivated)
                return ResultPattern.SuccessResult("Already Activated");
            var isValid = await _vendorRepository.CheckVendorVerificationCodeAsync(vendor.VendorId);
            if (!isValid)
                ResultPattern.FailResult("Code expired or does not exist");
            vendor.IsActivated = true;
            await _vendorRepository.SaveChangesAsync();
            return ResultPattern.SuccessResult("Activation successfull");
        }

        public async Task<ResultPattern> UpdateVendorAsync(Guid vendorId, UpdateVendorDto updateVendor, HttpRequest request)
        {
            var vendor = await _vendorRepository.GetVendorByIdAsync(vendorId);
            if (vendor is null)
                return ResultPattern.FailResult("Update failed");
            await vendor.UpdateVendor(updateVendor);
            await _vendorRepository.SaveChangesAsync();
            var vendorView = MapToVendorViewDto(vendor, request);
            return ResultPattern.SuccessResult(vendorView);
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

        private static Vendor PrepareVendorSignUp(User user, CreateVendorDto dto)
        {
            return new Vendor
            {
                User = user,
                UserId = user.Id,
                VendorId = Guid.CreateVersion7(),
                VendorName = dto.Name,
                VendorEmail = dto.Email,
                VendorPhone = dto.Phone,
                Location = dto.Location,
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
                LogoUrl = GlobalConstants.GetImagetUrl(request, vendor.VendorLogoUri),
                BannerUrl = GlobalConstants.GetImagetUrl(request,vendor.VendorBannerUri),
                Location = vendor.Location,
                GoogleMapUrl = vendor.GoogleMapUrl,
                TwitterUrl = vendor.TwitterUrl,
                InstagramUrl = vendor.InstagramUrl,
                FacebookUrl = vendor.FacebookUrl,
                DateJoined = vendor.DateJoined,
            };
        }

        public async Task<PagedResponse<ProductViewDto>> GetVendorPagedProductsAsync(PagedResponseDto r)
        {
            int pageNumber = r.PageNumber < 1 ? 1 : r.PageNumber;
            int pageSize = r.PageSize > MaxPageSize ? MaxPageSize : r.PageSize;

            var (products, totalCount) = await _vendorRepository
                .GetVendorPagedProductAsync(r.VendorId, pageNumber, pageSize);

            var productDto = products.Select(p => MapProductToDto(p, r.Request));

            return new PagedResponse<ProductViewDto>(
                productDto,
                pageNumber,
                pageSize,
                totalCount
            );
        }

        private static ProductViewDto MapProductToDto(Product product, HttpRequest request)
        {
            var img = GlobalConstants.GetImagetUrl(request, product.ImageUrl);
            return new ProductViewDto
            {
                ProductId = product.ProductId,
                ProductName = product.Name,
                Price = product.Price,
                Description = product.Description,
                ImageUrl = img,
                Images = [img, img, img, img],
                VendorId = product.VendorId,
                VendorName = product.VendorName,
                Stock = product.StockQuantity,
                Rating = product.Rating,
                ReveiwCount = product.ReviewCount,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
            };
        }

    }
}