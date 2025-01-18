using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DataTransferObjects
{
    public class VendorViewDto
    {
        public int VendorId {get; set;}
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
        public DateTime DateJoined { get; set; }
        public DateTime? DateUpdated { get; set; }
        public ICollection<ProductViewDto>? Products { get; set; }

    }
}
