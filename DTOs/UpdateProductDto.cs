namespace AspNetCoreEcommerce.DTOs
{
    public class UpdateProductDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public double Price { get; set; }
        public string[] Categories { get; set; } = new string[3];
    }
}
