namespace AspNetCoreEcommerce.Entities
{
    public class Product
    {
        public Guid ProductId {get; set;}
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public double Price { get; set; }
        public Guid CategoryId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Category? Category { get; set; }
        public Guid VendorId { get; set; }
        public Vendor? Vendor { get; set; }


        public void UpdateProduct(string? name, string? description, string? imageurl, double? price)
        {
            if (!string.IsNullOrWhiteSpace(name))
                Name = name;

            if (!string.IsNullOrWhiteSpace(description))
                Description = description;

            if (!string.IsNullOrWhiteSpace(imageurl))
                ImageUrl = imageurl;

            if (price != null)
                Price = (double)price;
        }
    }
}
