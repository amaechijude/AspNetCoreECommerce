using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.Domain.Entities
{
    public class ProductTags
    {
        [Key]
        public int TagId { get; set; }
        public required string TagName { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; } = [];
    }
    
}