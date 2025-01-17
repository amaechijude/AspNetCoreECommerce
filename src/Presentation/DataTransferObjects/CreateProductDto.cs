using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Microsoft.AspNetCore.Http;

namespace DataTransferObjects
{
    public class CreateProductDto
    {
        public int VendorId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public double Price { get; set; }
        public string[] Categories { get; set; } = new string[3];
    }
}
