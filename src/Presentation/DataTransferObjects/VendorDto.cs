using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DataTransferObjects
{
    public class VendorDto
    {
        [Required]
        public string? VendorName { get; set; }
        [EmailAddress]
        public string? VendorEmail { get; set; }
        [StringLength(16)]
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
}
