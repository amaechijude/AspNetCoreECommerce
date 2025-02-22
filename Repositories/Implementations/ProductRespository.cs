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

        public async Task<IEnumerable<ProductViewDto>> GetAllProductsAsync(HttpRequest request)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return await _context.Products
                .Select(p => new ProductViewDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    ImageUrl = GlobalConstants.GetImagetUrl(request, p.ImageUrl),
                    Price = p.Price,
                    VendorId = p.VendorId,
                    VendorName = p.Vendor.VendorName
                })
                .ToListAsync();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
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

        public async Task DeleteProductAsync(Guid vendorId, Guid productId)
        {
            var vendor = await GetVendorByIdAsync(vendorId);
            var product = await _context.Products.FindAsync(productId)
                ?? throw new KeyNotFoundException($"Product with ID {productId} not found or deleted.");

            if (vendor.VendorId != product.VendorId)
                throw new UnauthorizedAccessException("You are non authorized");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Vendor> GetVendorByIdAsync(Guid vendorId)
        {
            var vendor = await _context.Vendors.FindAsync(vendorId)
                ?? throw new KeyNotFoundException("Vendor does not exist");
            return vendor;
        }

    }
}
