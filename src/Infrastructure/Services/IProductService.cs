using DataTransferObjects;
using Microsoft.AspNetCore.Http;

namespace Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewDto>> GetAllProductsAsync();
        Task<ProductViewDto> GetProductByIdAsync(int productId);
        Task<ProductViewDto> CreateProductAsync(CreateProductDto productDto, HttpRequest request);
        // Task<ProductViewDto> UpdateProductAsync(int productId, UpdateProductDto productDto);
        Task DeleteProductAsync(int productId);
    }
}
