using System.ComponentModel.DataAnnotations;

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
    public class CreateProductDto
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Image Required")]
        public IFormFile? Image { get; set; }
        [Required, Range(1.0, 10_000_000)]
        public decimal Price { get; set; }
    }

    public class ProductViewDto
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public List<string> Images { get; set; } = [];
        public decimal Price { get; set; }
        public required int Stock {get; set;}
        public Guid VendorId { get; set; }
        public string? VendorName { get; set; }
        public int Rating { get; set; }
        public int ReveiwCount { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool IsNew => DateTimeOffset.UtcNow - CreatedAt < TimeSpan.FromDays(30);
        public ICollection<RelatedProductDto> RelatedProducts { get; set; } = [];
    }

    public class RelatedProductDto
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
    }


    public class UpdateProductDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public decimal Price { get; set; }
    }
}
