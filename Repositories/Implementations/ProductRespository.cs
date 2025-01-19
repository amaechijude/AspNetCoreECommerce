using AspNetCoreEcommerce.Data;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Respositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreEcommerce.Respositories.Implementations
{
    public class ProductRepository(ApplicationDbContext context) : IProductRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            return product ?? throw new KeyNotFoundException($"Product with ID {productId} not found.");
        }

        public async Task<Product> CreateProductAsync(Product product, HttpRequest request)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }
        public async Task<Product> UpdateProductAsync(int productId, Product product)
        {
            _ = await _context.Products.FindAsync(productId) ?? throw new KeyNotFoundException($"Product with ID {productId} not found or deleted.");
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId) ?? throw new KeyNotFoundException($"Product with ID {productId} not found or deleted."); ;
            product.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<Category> GetCategorytByNameAsync(string name)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(cat => cat.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

            if (category == null)
            {
                var newCategory = new Category { Name = name}; 
                return newCategory;
            }
            return category;
        }

        public async Task<Vendor?> GetVendorByIdAsync(int vendorId)
        {
            var vendor = await _context.Vendors.FindAsync(vendorId);
            return vendor is null ? null : vendor;
        }

        public async Task<string?> SaveProductImageAsync(IFormFile imageFile, HttpRequest request)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Products");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = $"{Guid.NewGuid()}_{Path.GetExtension(imageFile.FileName)}".Replace(" ", "");
            var filePath = Path.Combine(uploadPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            var imageUrl = $"{request.Scheme}://{request.Host}/Uploads/Posts/{fileName}";
            return imageUrl;
        }
    }
}
