using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreEcommerce.DTOs
{
    public class LoginDto
    {
        [EmailAddress]
        [Required]
        public string? Email {get; set;}

        [Required]
        public string? Password {get; set;}
    }

    public class VendorLoginViewDto
    {
        public Guid VendorId {get; set;}
        public required string VendorEmail {get; set;}
        public required string Token {get; set;}
    }
    
    public class CustomerLoginViewDto
    {
        public Guid CustomerId {get; set;}
        public required string CustomerEmail {get; set;}
        public required string Token {get; set;}
    }
}