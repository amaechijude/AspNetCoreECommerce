using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.Respositories.Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int productId);
        Task<Product> CreateProductAsync(Product product, HttpRequest request);
        Task<Product> UpdateProductAsync(int productId, Product product);
        Task DeleteProductAsync(int productId);
        Task<Category> GetCategorytByNameAsync(string catName);
        Task<Vendor?> GetVendorByIdAsync(int vendorId);
        Task<string?> SaveProductImageAsync(IFormFile imageFile, HttpRequest request);
    }
}
