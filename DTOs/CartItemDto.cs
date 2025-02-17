using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.DTOs
{
    public class CustomerCartViewDto
    {
        public ICollection<ProductViewDto> Products { get; set; } = [];
    }

    public class CartItemDto
    {
        [Required]
        public Guid ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public class ReturnCartViewDto
    {
        public int CartProductCount { get; set; }
        public double CartTotalAmount { get; set; }
    }
}
