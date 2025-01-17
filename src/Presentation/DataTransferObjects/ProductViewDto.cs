using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DataTransferObjects
{
    public class ProductViewDto
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public double Price { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int VendorId { get; set; }
    }
}
