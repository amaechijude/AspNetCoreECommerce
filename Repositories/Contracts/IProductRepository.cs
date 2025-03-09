using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.Repositories.Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductViewDto>> GetAllProductsAsync(HttpRequest request);
        Task<Product?> GetProductByIdAsync(Guid productId);
        Task<Product> CreateProductAsync(Product product, HttpRequest request);
        Task UpdateProductAsync();
        Task DeleteProductAsync(Guid vendorId, Guid productId);
        Task<Vendor?> GetVendorByIdAsync(Guid vendorId);
    }
}
