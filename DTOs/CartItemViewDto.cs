namespace AspNetCoreEcommerce.DTOs
{
    public class CartItemViewDto
    {
        public Guid CustomerId {get; set;}
        public string? CustomerName {get; set;}
        public double TotalPrice {get; set;}
        public ICollection<ProductViewDto> Products {get; set;} = [];
    }
}