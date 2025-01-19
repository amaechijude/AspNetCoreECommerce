using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.DTOs
{
    public class CartViewDto
    {
        public ICollection<ProductViewDto> Products { get; set; } = [];
    }

    public class CartDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int CustomerId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }
    }
}
