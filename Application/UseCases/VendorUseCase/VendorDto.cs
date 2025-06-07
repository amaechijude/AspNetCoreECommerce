using System.ComponentModel.DataAnnotations;
using AspNetCoreEcommerce.Application.UseCases.ProductUseCase;

namespace AspNetCoreEcommerce.Application.UseCases.VendorUseCase
{
    public class UpdateVendorDto
    {
        public string? VendorPhone { get; set; }
        public IFormFile? VendorBanner { get; set; }
        public string? Location { get; set; }
        [Url]
        public string? GoogleMapUrl { get; set; }
        [Url]
        public string? TwitterUrl { get; set; }
        [Url]
        public string? InstagramUrl { get; set; }
        [Url]
        public string? FacebookUrl { get; set; }
    }

    public class VendorViewDto
    {
        public Guid VendorId { get; set; }
        public required string VendorName { get; set; }
        public required string VendorEmail { get; set; }
        public required string VendorPhone { get; set; }
        public string BannerUrl { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public required string Location { get; set; }
        public string GoogleMapUrl { get; set; } = string.Empty;
        public string TwitterUrl { get; set; } = string.Empty;
        public string InstagramUrl { get; set; } = string.Empty;
        public string FacebookUrl { get; set; } = string.Empty;
        public DateTimeOffset DateJoined { get; set; }
    }

    public class PagedResponseDto
    {
        public required Guid VendorId { get; set; }
        public required int PageNumber { get; set; } = 1;
        public required int PageSize { get; set; } = 50;
        public required HttpRequest Request { get; set; }
    }

    public class CreateVendorDto
    {
        [Required(ErrorMessage = "Vendor name is required")]
        public required string Name { get; set; }
        [Required, EmailAddress]
        public required string Email { get; set; }
        [Required, Phone]
        public required string Phone { get; set; }
        [Required]
        public required string Location { get; set; }
        public IFormFile? Logo { get; set; }
        public IFormFile? Banner { get; set; }
    }

    public class ActivateVendorDto
    {
        [Required, EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Code {get; set; }
    }
}
