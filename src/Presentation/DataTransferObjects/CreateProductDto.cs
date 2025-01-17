using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using Microsoft.AspNetCore.Http;

namespace DataTransferObjects
{
    public class CreateProductDto
    {
        [Required]
        public int VendorId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string? CategoryName { get; set; }
    }
}
