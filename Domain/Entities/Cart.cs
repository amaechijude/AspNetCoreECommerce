using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCoreEcommerce.Domain.Entities
{
    public class Cart
    {
        [Key]
        public Guid CartId {get; set;}
        public Guid UserId {get; set;}
        public required User User {get; set;}
        public DateTimeOffset CreatedAt {get; set;}
        public DateTimeOffset UpdatedAt {get; set;}
        public ICollection<CartItem> CartItems {get; set;} = [];
        public int CartItemsCount {get; set;}
        [Column(TypeName = "decimal(18,2)")]
        public decimal CartTotalAmount {get; set;}
    }
}