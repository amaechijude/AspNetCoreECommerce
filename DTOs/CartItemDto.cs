using System.ComponentModel.DataAnnotations;
using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.DTOs
{
    public class CartViewDto
    {
        public Guid CartId {get; set;}
        public int CartProductCount { get; set; }
        public double CartTotalAmount{ get; set; }
        public ICollection<CartItemViewDto> CartItems {get; set;} = [];
        
    }

    
    public class CartItemViewDto
    {
        [Required]
        public Guid CartItemId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice {get; set;}
        public double TotalPrice {get; set;}
        public Guid ProductId {get; set;}
    }

    public class AddToCartDto
    {
        [Required(ErrorMessage = "Product id Required")]
        public required string ProductId {get; set;}
        public int Quantity {get; set;} = 1;
    }

}
