using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.Respositories.Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(Guid productId);
        Task<Product> CreateProductAsync(Product product, HttpRequest request);
        Task UpdateProductAsync();
        Task DeleteProductAsync(Guid productId);
        Task<Category> GetCategorytByNameAsync(string catName);
        Task<Vendor> GetVendorByIdAsync(Guid vendorId);
        // Task<string?> SaveProductImageAsync(IFormFile imageFile, HttpRequest request);
    }
}
