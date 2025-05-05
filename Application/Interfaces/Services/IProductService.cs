using AspNetCoreEcommerce.Application.UseCases.ProductUseCase;
using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<ResultPattern> GetAllProductsAsync(HttpRequest request);
         Task<PagedResponse<ProductViewDto>> GetPagedProductsAsync(int pageNumber, int pageSize, HttpRequest httpRequest);
        Task<ResultPattern> GetProductByIdAsync(Guid productId, HttpRequest request);
        Task<ResultPattern> CreateProductAsync(Guid vendorId, CreateProductDto productDto, HttpRequest request);
        Task DeleteProductAsync(Guid vendorId, Guid productId);
        Task<ResultPattern> UpdateProductAsync(Guid vendorId, Guid productId, UpdateProductDto updateProduct, HttpRequest request);
    }
}
