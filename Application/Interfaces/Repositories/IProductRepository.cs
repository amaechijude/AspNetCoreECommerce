using AspNetCoreEcommerce.Application.UseCases.ProductUseCase;
using AspNetCoreEcommerce.Domain.Entities;

namespace AspNetCoreEcommerce.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductViewDto>> GetAllProductsAsync(HttpRequest request);
        Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedProductAsync(int pageNumber, int PageSize);
        Task<Product?> GetProductByIdAsync(Guid productId);
        Task<Product> CreateProductAsync(Product product, HttpRequest request);
        Task UpdateProductAsync();
        Task DeleteProductAsync(Guid vendorId, Guid productId);
        Task<Vendor?> GetVendorByIdAsync(Guid vendorId);
        Task<bool> CreateReviewAsync(Reveiw reveiw);
        Task SaveChangesAsync();
        Task<bool> CheckExistingReviewAsync(Guid productId, Guid userId);
    }
}
