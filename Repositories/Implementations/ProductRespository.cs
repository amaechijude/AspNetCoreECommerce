using AspNetCoreEcommerce.Data;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Respositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreEcommerce.Repositories.Implementations
{
    public class ProductRepository(ApplicationDbContext context) : IProductRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = await _context.Products.ToListAsync();
            return products;
        }

        public async Task<Product> GetProductByIdAsync(Guid productId)
        {
            var product = await _context.Products.FindAsync(productId)
                ?? throw new KeyNotFoundException($"Product with ID {productId} not found.");
            return product;
        }

        public async Task<Product> CreateProductAsync(Product product, HttpRequest request)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }
        public async Task UpdateProductAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Guid productId)
        {
            var product = await _context.Products.FindAsync(productId)
                ?? throw new KeyNotFoundException($"Product with ID {productId} not found or deleted."); ;
            product.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<Category> GetCategorytByNameAsync(string name)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(cat => cat.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

            if (category == null)
            {
                var newCategory = new Category { CategoryId = Guid.CreateVersion7(), Name = name };
                return newCategory;
            }
            return category;
        }

        public async Task<Vendor> GetVendorByIdAsync(Guid vendorId)
        {
            var vendor = await _context.Vendors.FindAsync(vendorId)
                ?? throw new KeyNotFoundException("Vendor does not exist");
            return vendor;
        }

        public async Task<string?> SaveProductImageAsync(IFormFile imageFile, HttpRequest request)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            var subPath = GlobalConstants.productSubPath;
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), GlobalConstants.uploadPath, subPath);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = $"{Guid.NewGuid()}_{Path.GetExtension(imageFile.FileName)}".Replace(" ", "");
            var filePath = Path.Combine(uploadPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            return $"{GlobalConstants.uploadPath}/{subPath}/{fileName}";
        }

    }
}
