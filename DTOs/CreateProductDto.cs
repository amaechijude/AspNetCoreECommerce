using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.DTOs
{
    public class CreateProductDto
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Image Required")]
        public IFormFile? Image { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
