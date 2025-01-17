using Data;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            return product ?? throw new KeyNotFoundException($"Product with ID {productId} not found.");
        }

        public async Task<Product> CreateProductAsync(Product product)
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
    }
}
