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
        [Required]
        public string? VendorName { get; set; }
        [EmailAddress]
        public string? VendorEmail { get; set; }
        [StringLength(16)]
        public string? VendorPhone { get; set; }
        public string? VendorBannerUrl { get; set; }
        public string? Location { get; set; }
        [Url]
        public string? GoogleMapUrl { get; set; }
        [Url]
        public string? TwitterUrl { get; set; }
        [Url]
        public string? InstagramUrl { get; set; }
        [Url]
        public string? FacebookUrl { get; set; }
        public DateTimeOffset DateJoined { get; set; }
        public DateTimeOffset? DateUpdated { get; set; }
        public DateTimeOffset LastLoginDate { get; set; }
        public ICollection<ProductViewDto>? Products { get; set; }

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
    }
}
