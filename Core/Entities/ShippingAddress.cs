using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    internal class ShippingAddress
    {
        [Key]
        public int Id { get; set; }
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
        public Customer? Customer { get; set; }

        public ShippingAddress() { }
    }
}
