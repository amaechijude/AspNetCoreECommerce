using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCoreEcommerce.Entities
{
    public class Product
    {
        [Key]
        public Guid ProductId {get; set;}
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        [Range(0.01, 10_000_000, ErrorMessage = "Price must be between 0.01 and 10,000,000")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int StockQuantity {get; set;}
        public bool IsAvailable {get; set;}
        [Range(0, 100, ErrorMessage = "Discount percentage must by between 0 and 100")]
        [Column(TypeName = "decimal(4,2)")]
        public decimal DiscountPercentage {get; set;}
        [Required]
        public required DateTimeOffset CreatedAt {get; set;}
        public DateTimeOffset UpdateddAt {get; set;}
        [Required]
        public required Guid VendorId { get; set; }
        public string? VendorName {get; set;}
        public Vendor? Vendor { get; set; }
        public ICollection<CartItem>? CartItems { get; set; }
        public ICollection<OrderItem> OrderItems {get; set;} = [];
        public ICollection<Feedback>? Feedbacks {get; set;}

        public void UpdateProduct(Vendor vendor, Guid vendorId, string? name, string? description, string? imageName, decimal? price)
        {
            if (!string.IsNullOrWhiteSpace(name))
                ProductName = name;

            if (!string.IsNullOrWhiteSpace(description))
                Description = description;

            if (!string.IsNullOrWhiteSpace(imageName))
                ImageUrl = imageName;

            if (price != null)
                Price = (decimal)price;

            Vendor = vendor;
            VendorId = vendorId;
            UpdateddAt = DateTimeOffset.UtcNow;
        }
    }
}
