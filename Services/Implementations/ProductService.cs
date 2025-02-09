using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Repositories.Contracts;
using AspNetCoreEcommerce.Services.Contracts;

namespace AspNetCoreEcommerce.Services.Implementations
{
    // Services/ProductService.cs
    public class ProductService(IProductRepository productRepository) : IProductService
    {
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<IEnumerable<ProductViewDto>> GetAllProductsAsync(HttpRequest request)
        {
            return await _productRepository.GetAllProductsAsync(request);
        }

        public async Task<ProductViewDto> GetProductByIdAsync(Guid productId, HttpRequest request)
        {
            var product = await _productRepository.GetProductByIdAsync(productId)
                ?? throw new KeyNotFoundException("Product not found or is deleted");

            return MapProductToDto(product, request);
        }

        public async Task<ProductViewDto> CreateProductAsync(Guid vendorId, CreateProductDto createProductDto, HttpRequest request)
        {
            if (string.IsNullOrWhiteSpace(createProductDto.Name))
                throw new ArgumentException("Product name cannot be empty.");

            if (createProductDto.Price <= 0)
                throw new ArgumentException("Product price must be greater than zero.");

            if (createProductDto.Image is null)
                throw new ArgumentException("Product Image is missing");

            var imageUrl = await GlobalConstants.SaveImageAsync(createProductDto.Image, GlobalConstants.productSubPath);
            var vendor = await _productRepository.GetVendorByIdAsync(vendorId);

            var product = new Product
            {
                ProductId = Guid.CreateVersion7(),
                Name = createProductDto.Name,
                Price = createProductDto.Price,
                Description = createProductDto.Description,
                VendorId = vendor.VendorId,
                Vendor = vendor,
                ImageName = imageUrl
            };
            // category.Products.Add(product);
            var createdProduct = await _productRepository.CreateProductAsync(product, request);

            return MapProductToDto(createdProduct, request);
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

            return MapProductToDto(product, request);
        }

        private static ProductViewDto MapProductToDto (Product product, HttpRequest request)
        {
            return new ProductViewDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                ImageUrl = GlobalConstants.GetImagetUrl(request, product.ImageName),
                VendorId = product.VendorId,
                VendorName = product.Vendor.VendorName
            };
        }

    }

}
