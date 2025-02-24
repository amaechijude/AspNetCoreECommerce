using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCoreEcommerce.Entities
{
    public class Cart
    {
        [Key]
        public Guid CartId {get; set;}
        public Guid CustomerId {get; set;}
        public required Customer Customer {get; set;}
        public bool IsCheckedOut {get; set;}
        public DateTimeOffset CreatedAt {get; set;}
        public DateTimeOffset UpdatedAt {get; set;}
        public ICollection<CartItem> CartItems {get; set;} = [];
        public int CartItemsCount {get; set;}
        [Column(TypeName = "decimal(18,2)")]
        public decimal CartTotalAmount {get; set;}
    }
}