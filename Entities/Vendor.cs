using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.Entities
{
    public class Vendor
    {
        public Guid VendorId { get; set; }
        public string? VendorName { get; set; }
        [EmailAddress]
        public required string VendorEmail { get; set; }
        [Required]
        [PasswordPropertyText]
        public string? PasswordHash {get; set;}
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
        public DateTimeOffset DateJoined { get; set; }
        public DateTimeOffset DateUpdated { get; private set; }
        public ICollection<Product>? Products { get; set; }

        public void UpdateVendor(string? vphone, string? location, string? gmap, string? xurl, string? igurl, string? fburl)
        {
            if (!string.IsNullOrWhiteSpace(vphone))
                VendorPhone = vphone;
            if (!string.IsNullOrWhiteSpace(location))
                Location = location;
            if (!string.IsNullOrWhiteSpace(gmap))
                GoogleMapUrl = gmap;
            if (!string.IsNullOrWhiteSpace(xurl))
                TwitterUrl = xurl;
            if (!string.IsNullOrWhiteSpace(igurl))
                InstagramUrl = igurl;
            if (!string.IsNullOrWhiteSpace(fburl))
                FacebookUrl = fburl;

            DateUpdated = DateTimeOffset.UtcNow;
        }
    }
}
