using AspNetCoreEcommerce.Data;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Repositories.Contracts;
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
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var category = await _context.Categories
                .FirstOrDefaultAsync(cat => cat.Name.ToLower() == name.ToLower());
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            if (category == null)
            {
                var newCategory = new Category { CategoryId = Guid.NewGuid(), Name = name };
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

    }
}
