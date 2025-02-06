using System.ComponentModel.DataAnnotations;
namespace AspNetCoreEcommerce.DTOs
{
    public class UpdateVendorDto
    {
        public string? VendorPhone {get; set;}
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
}
