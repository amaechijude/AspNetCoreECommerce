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
        [Required, MinLength(3), StringLength(200)]
        public required string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public IFormFile? Image { get; set; }

        [Required, Range(0.01, 1_000_000_000_000_000, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
        public int StockQuantity { get; set; }

        [Range(0, 100, ErrorMessage = "Discount percentage must be between 0 and 100")]
        public decimal DiscountPercentage { get; set; } = 0;
    }
    public class PagedProductResponseDto
    {
        public required int PageNumber { get; set; } = 1;
        public required int PageSize { get; set; } = 50;
        public required HttpRequest Request { get; set; }
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
        public decimal? Price { get; set; }
    }
}
