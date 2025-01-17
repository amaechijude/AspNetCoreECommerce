using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public double Price { get; set; }
        public int? CategoryId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Category? Category { get; set; }
        public int VendorId { get; set; }
        public Vendor? Vendor { get; set; }
    }
}
