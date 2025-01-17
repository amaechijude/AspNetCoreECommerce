using Entities;

namespace Repositories
{
    internal interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int productId);
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(int productId, Product product);
        Task DeleteProductAsync(int productId);
        Task SaveChangesAsync();
    }
}
