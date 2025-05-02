using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.Application.UseCases.CartUseCase
{
    public class CartViewDto
    {
        public Guid CartId {get; set;}
        public Guid UserId {get; set;}
        public string UserEmail {get; set;} = string.Empty;
        public string UserName {get; set;} = string.Empty;
        public int CartProductCount { get; set; }
        public decimal CartTotalAmount{ get; set; }
        public ICollection<CartItemViewDto> CartItems {get; set;} = [];
        
    }

    
    public class CartItemViewDto
    {
        [Required]
        public Guid CartItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice {get; set;}
        public decimal TotalPrice {get; set;}
        public Guid ProductId {get; set;}
        public string ProductName {get; set;} = string.Empty;
        public string VendorName {get; set;} = string.Empty;
    }

    public class RemoveFromCartViewDto
    {
        public bool Success {get; set;}
        public required string Message {get; set;}
    }

    public class AddToCartDto
    {
        [Required(ErrorMessage = "Product id Required")]
        public required string ProductId {get; set;}
        public int Quantity {get; set;} = 1;
    }

}
