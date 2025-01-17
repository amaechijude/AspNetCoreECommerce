using Repositories;
using DataTransferObjects;

namespace Services
{
    // Services/ProductService.cs
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductViewDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return products.Select(p => new ProductViewDto
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                VendorId = p.VendorId,
                Category = p.Category,

                // ... map other properties
            }).ToList();
        }

        public async Task<ProductViewDto> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null)
                throw new KeyNotFoundException("Product not found or is deleted");

            return new ProductViewDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                VendorId = product.VendorId,
                Category = product.Category,
            };
        }

        // Implement other methods (CreateProductAsync, UpdateProductAsync, DeleteProductAsync)
        // with similar manual mapping logic
    }
}
