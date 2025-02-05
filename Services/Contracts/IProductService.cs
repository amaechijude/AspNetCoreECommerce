using AspNetCoreEcommerce.DTOs;

namespace AspNetCoreEcommerce.Services.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewDto>> GetAllProductsAsync();
        Task<ProductViewDto> GetProductByIdAsync(Guid productId);
        Task<ProductViewDto> CreateProductAsync(string vendorId, CreateProductDto productDto, HttpRequest request);
        // Task<ProductViewDto> UpdateProductAsync(int productId, UpdateProductDto productDto);
        Task DeleteProductAsync(Guid vendorId, Guid productId);
    }
}
