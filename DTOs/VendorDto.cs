using System.ComponentModel.DataAnnotations;
namespace AspNetCoreEcommerce.DTOs
{
    public class VendorDto
    {
        [Required]
        public string? VendorName { get; set; }
        [Required]
        [EmailAddress]
        public string? VendorEmail { get; set; }
        [Required]
        public string? Password {get; set; }
        [Required]
        [StringLength(16)]
        public string? VendorPhone { get; set; }
        public IFormFile? VendorBanner { get; set; }
        [Required]
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
}
