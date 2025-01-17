using Entities;

namespace Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int productId);
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(int productId, Product product);
        Task DeleteProductAsync(int productId);
    }
}
