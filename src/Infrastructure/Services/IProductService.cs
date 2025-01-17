using DataTransferObjects;

namespace Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductByIdAsync(int productId);
        Task<ProductDto> CreateProductAsync(CreateProductDto productDto);
        Task<ProductDto> UpdateProductAsync(int productId, UpdateProductDto productDto);
        Task DeleteProductAsync(int productId);
        Task SaveChangesAsync();
    }
}
