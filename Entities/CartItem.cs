
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.Entities
{
    public class CartItem
    {
        [Key]
        public Guid CartItemId { get; set; }
        public Guid CartId {get; set;}
        public required Cart Cart {get; set;}
        public Guid ProductId { get; set; }
        public required Product Product { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice {get; set; }
        public double TotalPrice {get; set;}
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
