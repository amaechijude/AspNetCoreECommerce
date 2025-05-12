
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCoreEcommerce.Domain.Entities
{
    public class CartItem
    {
        [Key]
        public Guid CartItemId { get; set; }
        public Guid CartId {get; set;}
        public Cart? Cart {get; set;}
        public Guid ProductId { get; set; }
        public required Product Product { get; set; }
        public string? ProductName {get; set;}
        public Guid VendorId {get; set;}
        public string? VendorName {get; set;}
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice {get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice => UnitPrice * Quantity;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public void UpdateCartItem(int quantity)
        {
            Quantity = quantity;
            UpdatedAt = DateTimeOffset.UtcNow;
            return;
        }
    }
}
