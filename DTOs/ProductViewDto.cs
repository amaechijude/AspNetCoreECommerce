namespace AspNetCoreEcommerce.DTOs
{
    public class ProductViewDto
    {
        public Guid ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public double Price { get; set; }
        public Guid CategoryId { get; set; }
        public Guid VendorId { get; set; }
        public string? VendorName {get; set;}
    }
}
