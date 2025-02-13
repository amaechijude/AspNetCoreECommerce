using AspNetCoreEcommerce.DTOs;

namespace AspNetCoreEcommerce.Services.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewDto>> GetAllProductsAsync(HttpRequest request);
        Task<ProductViewDto> GetProductByIdAsync(Guid productId, HttpRequest request);
        Task<ProductViewDto> CreateProductAsync(Guid vendorId, CreateProductDto productDto, HttpRequest request);
        // Task<ProductViewDto> UpdateProductAsync(int productId, UpdateProductDto productDto);
        Task DeleteProductAsync(Guid vendorId, Guid productId);
        Task<ProductViewDto> UpdateProductAsync(Guid vendorId, Guid productId, UpdateProductDto updateProduct, HttpRequest request);
    }
}
