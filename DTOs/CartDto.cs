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
        public Guid CustomerId { get; set; }
        [Required]
        public Guid ProductId { get; set; }
    }
}
