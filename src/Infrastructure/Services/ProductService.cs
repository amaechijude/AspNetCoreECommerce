using Repositories;
using DataTransferObjects;
using Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography.X509Certificates;
using System.Numerics;


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
                // CategoryName = p.Category.Name;

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
                // Category = product.Category,
            };
        }

        public async Task<ProductViewDto> CreateProductAsync(CreateProductDto createProductDto, HttpRequest request)
        {
            if (string.IsNullOrWhiteSpace(createProductDto.Name))
                throw new ArgumentException("Product name cannot be empty.");

            if (string.IsNullOrWhiteSpace(createProductDto.CategoryName))
                throw new ArgumentException("Category name cannot be empty.");

            if (createProductDto.Price <= 0)
                throw new ArgumentException("Product price must be greater than zero.");
            if (createProductDto.Image is null)
                throw new Exception("Product Image is missing");

            var imageUrl = await _productRepository.SaveProductImageAsync(createProductDto.Image, request);
            var vendor = await _productRepository.GetVendorByIdAsync(createProductDto.VendorId) ?? throw new Exception("The vendor is not found. Contact the admin");
            var category = await _productRepository.GetCategorytByNameAsync(createProductDto.CategoryName);
            var product = new Product
            {
                Name = createProductDto.Name,
                Price = createProductDto.Price,
                Description = createProductDto.Description,
                Category = category,
                Vendor = vendor,
                ImageUrl = imageUrl
            };

            var createdProduct = await _productRepository.CreateProductAsync(product, request);
            // await _productRepository.SaveChangesAsync
            return new ProductViewDto
            {
                ProductId = createdProduct.ProductId,
                Name = createdProduct.Name,
                Price = createdProduct.Price,
                Description = createdProduct.Description,
                ImageUrl = createdProduct.ImageUrl,
                VendorId = createdProduct.VendorId,
                // CategoryName = createdProduct.Category.Name,
            };
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product  = await _productRepository.GetProductByIdAsync(curre)
        }
    }

}
