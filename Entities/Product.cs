namespace AspNetCoreEcommerce.Entities
{
    public class Product
    {
        public Guid ProductId {get; set;}
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageName { get; set; }
        public double Price { get; set; }
        // public bool IsDeleted { get; set; } = false;
        public Guid VendorId { get; set; }
        public required Vendor Vendor { get; set; }

        public void UpdateProduct(Vendor vendor, Guid vendorId, string? name, string? description, string? imageName, double? price)
        {
            if (!string.IsNullOrWhiteSpace(name))
                Name = name;

            if (!string.IsNullOrWhiteSpace(description))
                Description = description;

            if (!string.IsNullOrWhiteSpace(imageName))
                ImageName = imageName;

            if (price != null)
                Price = (double)price;

            Vendor = vendor;
            VendorId = vendorId;
        }
    }
}
