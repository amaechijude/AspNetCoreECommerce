using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.Entities
{
    public class Vendor
    {
        public int VendorId { get; set; }
        public string? VendorName { get; set; }
        [EmailAddress]
        public required string VendorEmail { get; set; }
        [StringLength(16)]
        public string? VendorPhone {  get; set; }
        public string? VendorBannerUrl { get; set; }
        public string? Location { get; set; }
        public string Role = GlobalConstants.vendorRole;
        public bool IsDeleted = false;
        [Url]
        public string? GoogleMapUrl { get; set; }
        [Url]
        public string? TwitterUrl { get;set; }
        [Url]
        public string? InstagramUrl { get; set; }
        [Url]
        public string? FacebookUrl { get; set; }
        public DateTime DateJoined { get; set; }
        public DateTime? DateUpdated { get; set; }
        public ICollection<Product>? Products { get; set; }

    }
}
