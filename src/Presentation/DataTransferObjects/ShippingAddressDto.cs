using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class ShippingAddressDto
    {
        [Required]
        public string? FullName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [StringLength(16)]
        public string? Phone { get; set; }
        [Required]
        public string? Address { get; set; }
        public string? SecondAddress { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public int CustormerId { get; set; }
    }
}
