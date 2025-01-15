using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Vendor
    {
        [Key]
        public int VendorId { get; set; }
        public string? VendorName { get; set; }
        [EmailAddress]
        public string? VendorEmail { get; set; }
        [StringLength(16)]
        public string? VendorPhone {  get; set; }
        public string? VendorBannerUrl { get; set; }
        public string? Location { get; set; }
        [Url]
        public string? GoogleMapUrl { get; set; }
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
