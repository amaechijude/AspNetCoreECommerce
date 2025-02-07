using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.Entities
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        [Required]
        public string? Name { get; set; }
        public ICollection<Product>? Products { get; set; } = [];
    }
}
