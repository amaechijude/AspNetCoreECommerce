using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.DTOs
{
    public class CreateProductDto
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        [Required]
        public double Price { get; set; }
    }
}
