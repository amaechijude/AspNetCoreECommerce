using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCoreEcommerce.Domain.Entities
{
    public class Product
    {
        [Key]
        public Guid ProductId {get; set;}
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int StockQuantity {get; set;}
        public bool IsAvailable => StockQuantity > 0;
        [Range(0, 100, ErrorMessage = "Discount percentage must by between 0 and 100")]
        [Column(TypeName = "decimal(4,2)")]
        public decimal DiscountPercentage {get; set;}
        public required DateTimeOffset CreatedAt {get; set;}
        public DateTimeOffset UpdatedAt {get; set;}
        public required Guid VendorId { get; set; }
        public string? VendorName {get; set;}
        public Vendor? Vendor { get; set; }
        public int Rating { get; set; }
        public int ReviewCount { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = [];
        public ICollection<OrderItem> OrderItems {get; set;} = [];
        public ICollection<Review> Reviews { get; set; } = [];
        public ICollection<ProductTags> Tags { get; set; } = [];

        public void UpdateProduct(string? name, string? description, string? imageName, decimal? price)
        {
            if (!string.IsNullOrWhiteSpace(name))
                Name = name;

            if (!string.IsNullOrWhiteSpace(description))
                Description = description;

            if (!string.IsNullOrWhiteSpace(imageName))
                ImageUrl = imageName;

            if (price > 0)
                Price = (decimal)price;

            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}
