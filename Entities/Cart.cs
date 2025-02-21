using System.ComponentModel.DataAnnotations;

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
        public double CartTotalAmount {get; set;}
    }
}