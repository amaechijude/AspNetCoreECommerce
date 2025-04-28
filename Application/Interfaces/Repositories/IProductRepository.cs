using AspNetCoreEcommerce.Application.UseCases.ProductUseCase;
using AspNetCoreEcommerce.Domain.Entities;

namespace AspNetCoreEcommerce.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductViewDto>> GetAllProductsAsync(HttpRequest request);
        Task<Product?> GetProductByIdAsync(Guid productId);
        Task<Product> CreateProductAsync(Product product, HttpRequest request);
        Task UpdateProductAsync();
        Task DeleteProductAsync(Guid vendorId, Guid productId);
        Task<Vendor?> GetVendorByIdAsync(Guid vendorId);
        Task SaveChangesAsync();
    }
}
