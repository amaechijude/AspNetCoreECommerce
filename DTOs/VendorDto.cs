using System.ComponentModel.DataAnnotations;
namespace AspNetCoreEcommerce.DTOs
{
    public class VendorDto
    {
        [Required(ErrorMessage = "Vendor name is required")]
        public string? VendorName { get; set; }
        [Required(ErrorMessage = "Vendor Email is required")]
        [EmailAddress]
        public string? VendorEmail { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password {get; set; }
        [Required(ErrorMessage = "Vendor Phone is required")]
        [StringLength(16, ErrorMessage = "Phone number can't exceed 16 digits")]
        public string? VendorPhone { get; set; }
        [Required(ErrorMessage = "Vendor Location is required")]
        public string? Location { get; set; }
        public IFormFile? VendorBanner { get; set; }
        [Url]
        public string? GoogleMapUrl { get; set; }
        [Url]
        public string? TwitterUrl { get; set; }
        [Url]
        public string? InstagramUrl { get; set; }
        [Url]
        public string? FacebookUrl { get; set; }
    }
}
