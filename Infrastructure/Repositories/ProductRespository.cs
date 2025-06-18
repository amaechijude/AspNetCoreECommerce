using AspNetCoreEcommerce.Application.Interfaces.Repositories;
using AspNetCoreEcommerce.Application.UseCases.ProductUseCase;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Infrastructure.Data;
using AspNetCoreEcommerce.Shared;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreEcommerce.Infrastructure.Repositories
{
    public class ProductRepository(ApplicationDbContext context, ILogger<ProductRepository> logger) : IProductRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger<ProductRepository> _logger = logger;   

        public async Task<IEnumerable<ProductViewDto>> GetAllProductsAsync(HttpRequest request)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return await _context.Products
                .Select(p => new ProductViewDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.Name,
                    Description = p.Description,
                    ImageUrl = GlobalConstants.GetImagetUrl( request, p.ImageUrl),
                    Price = p.Price,
                    VendorId = p.VendorId,
                    VendorName = p.Vendor.VendorName,
                    Stock = p.StockQuantity
                })
                .ToListAsync();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public async Task<Product?> GetProductByIdAsync(Guid productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task<Product> CreateProductAsync(Product product, HttpRequest request)
        {
            try
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return product;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task UpdateProductAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<bool> CheckExistingReviewAsync(Guid productId, Guid userId)
        {
            var review = await _context.Reviews
                .Where(r => r.ProductId == productId && r.UserId == userId)
                .FirstOrDefaultAsync();
            return review is not null;
        }

        public async Task DeleteProductAsync(Guid vendorId, Guid productId)
        {
            var vendor = await GetVendorByIdAsync(vendorId)
                ?? throw new KeyNotFoundException($"Vendor with ID {vendorId} not found or deleted.");
            var product = await _context.Products.FindAsync(productId)
                ?? throw new KeyNotFoundException($"Product with ID {productId} not found or deleted.");

            if (vendor.VendorId != product.VendorId)
                throw new UnauthorizedAccessException("You are non authorized");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Vendor?> GetVendorByIdAsync(Guid vendorId)
        {
            var vendor = await _context.Vendors.FindAsync(vendorId);
            return vendor is null ? null : vendor;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedProductAsync(int pageNumber, int pageSize)
        {
            var query = _context.Products.AsQueryable();

            var totalCount = await query.CountAsync();
            var items = await query
                .AsNoTracking()
                .OrderBy(p => p.ProductId)
                .ThenBy(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.Reviews)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<bool> CreateReviewAsync(Review reveiw)
        {
            try
            {
                await _context.Reviews.AddAsync(reveiw);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Reveiw not added");
                return false;
            }
        }

        public async Task<IEnumerable<ProductViewDto>> GetTrendingProducts(HttpRequest request)
        {
            return await _context.Products
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedAt)
                .Take(8)
                .Select(p => new ProductViewDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.Name,
                    Description = p.Description,
                    ImageUrl = GlobalConstants.GetImagetUrl(request, p.ImageUrl),
                    Price = p.Price,
                    VendorId = p.VendorId,
                    VendorName = p.VendorName,
                    Stock = p.StockQuantity
                })
                .ToListAsync();
        }
    }
}
