using System.ComponentModel.DataAnnotations;
using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.DTOs
{
    public class CustomerCartViewDto
    {
        public int CartProductCount { get; set; }
        public double CartTotalAmount{ get; set; }
        public ICollection<CsCartItemViewDto> CartItems { get; set; } = [];
    }

    public class CsCartItemViewDto
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double SubTotalPrice { get; set; }
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
