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
        public string? Name { get; set; }
        [Required(ErrorMessage = "Vendor Email is required"), EmailAddress]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Vendor Phone is required"), Phone]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Vendor Location is required")]
        public string? Location { get; set; }
        public IFormFile? Logo { get; set; }
    }
}
