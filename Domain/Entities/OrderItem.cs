using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCoreEcommerce.Domain.Entities
{
    public class OrderItem
    {
        [Key]
        public Guid OrderItemId { get; set; }
        public Guid OrdeId { get; set; }
        public Guid ProductId { get; set; }
        public required Product Product { get; set; }
        public string? ProductName { get; set; }
        public Guid VendorId { get; set; }
        public string? VendorName { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;

    }
}