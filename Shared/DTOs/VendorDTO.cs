using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    internal class VendorDTO
    {
        [Required]
        public string? VendorName { get; set; }
        [Required]
        [EmailAddress]
        public string? VendorEmail { get; set; }
        [Required]
        [StringLength(16)]
        public string? VendorPhone { get; set; }
        public string? VendorBanner { get; set; }
        [Required]
        public string? Location { get; set; }
        [Url]
        public string? GoogleMapUrl { get; set; }
        public string? TwitterUrl { get; set; }
        [Url]
        public string? InstagramUrl { get; set; }
        [Url]
        public string? FacebookUrl { get; set; }
    }
}
