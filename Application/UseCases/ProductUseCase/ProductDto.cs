namespace AspNetCoreEcommerce.Application.UseCases.ProductUseCase
{
    public class ProductDto
    {
        public Guid ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public Guid VendorId { get; set; }
    }
}
