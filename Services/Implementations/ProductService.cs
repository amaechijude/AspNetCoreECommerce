using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Respositories.Contracts;
using AspNetCoreEcommerce.Services.Contracts;
using Microsoft.AspNetCore.Mvc.Routing;

namespace AspNetCoreEcommerce.Services.Implementations
{
    // Services/ProductService.cs
    public class ProductService(IProductRepository productRepository) : IProductService
    {
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<IEnumerable<ProductViewDto>> GetAllProductsAsync(HttpRequest request)
        {
            var products = await _productRepository.GetAllProductsAsync();
            if (!products.Any())
                return [];

            return products
                .Select(p => new ProductViewDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    ImageUrl = GlobalConstants.GetImagetUrl(request, p.ImageName),
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                    VendorId = p.VendorId
            });
        }

        public async Task<ProductViewDto> GetProductByIdAsync(Guid productId, HttpRequest request)
        {
            var product = await _productRepository.GetProductByIdAsync(productId)
                ?? throw new KeyNotFoundException("Product not found or is deleted");

            return new ProductViewDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                ImageUrl = GlobalConstants.GetImagetUrl(request, product.ImageName),
                CategoryId = product.CategoryId,
                VendorId = product.VendorId
            };
        }

        public async Task<ProductViewDto> CreateProductAsync(Guid vendorId,CreateProductDto createProductDto, HttpRequest request)
        {
            if (string.IsNullOrWhiteSpace(createProductDto.Name))
                throw new ArgumentException("Product name cannot be empty.");

            if (string.IsNullOrWhiteSpace(createProductDto.CategoryName))
                throw new ArgumentException("Category name cannot be empty.");

            if (createProductDto.Price <= 0)
                throw new ArgumentException("Product price must be greater than zero.");

            if (createProductDto.Image is null)
                throw new ArgumentException("Product Image is missing");

            var imageUrl = await GlobalConstants.SaveImageAsync(createProductDto.Image, GlobalConstants.productSubPath);

            var category = await _productRepository.GetCategorytByNameAsync(createProductDto.CategoryName);
            var product = new Product
            {
                ProductId = Guid.CreateVersion7(),
                Name = createProductDto.Name,
                Price = createProductDto.Price,
                Description = createProductDto.Description,
                CategoryId = category.CategoryId,
                VendorId = vendorId,
                ImageName = imageUrl
            };

            var createdProduct = await _productRepository.CreateProductAsync(product, request);
            return new ProductViewDto
            {
                ProductId = createdProduct.ProductId,
                Name = createdProduct.Name,
                Price = createdProduct.Price,
                Description = createdProduct.Description,
                ImageUrl = GlobalConstants.GetImagetUrl(request, createdProduct.ImageName),
                CategoryId = createdProduct.CategoryId,
                // CategoryName = createdProduct.Category.Name,
                VendorId = createdProduct.VendorId,
            };
        }

        public async Task DeleteProductAsync(Guid vendorId, Guid productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (vendorId != product.VendorId)
                throw new UnauthorizedAccessException("You are not authorized for this action");
            
            await _productRepository.DeleteProductAsync(productId);
        }

        public async Task<ProductViewDto> UpdateProductAsync(Guid productId, Guid vendorId, UpdateProductDto updateProduct, HttpRequest request)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (vendorId != product.VendorId)
                throw new UnauthorizedAccessException("You are not authorized for this action");

            var imageUrl = updateProduct.Image == null
                ? null
                : await GlobalConstants.SaveImageAsync(updateProduct.Image, GlobalConstants.productSubPath);

            product.UpdateProduct(updateProduct.Name, updateProduct.Description, imageUrl, updateProduct.Price);
            await _productRepository.UpdateProductAsync();

            return new ProductViewDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                ImageUrl = GlobalConstants.GetImagetUrl(request, product.ImageName),
                CategoryId = product.CategoryId,
                VendorId = product.VendorId
            };
        }

    }

}
