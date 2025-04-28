using AspNetCoreEcommerce.Application.Interfaces.Repositories;
using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Application.UseCases.ProductUseCase
{
    // Services/ProductService.cs
    public class ProductService(IProductRepository productRepository) : IProductService
    {
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<ResultPattern> GetAllProductsAsync(HttpRequest request)
        {
            var data = await _productRepository.GetAllProductsAsync(request);
            return ResultPattern.SuccessResult(data, "Products fetched successfully");
        }

        public async Task<ResultPattern> GetProductByIdAsync(Guid productId, HttpRequest request)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);

            if (product is null)
                return ResultPattern.FailResult("Product not found", 404);
            
            return ResultPattern.SuccessResult(MapProductToDto(product, request), "Product found");
        }

        public async Task<ResultPattern> CreateProductAsync(Guid vendorId, CreateProductDto createProductDto, HttpRequest request)
        {
            if (string.IsNullOrWhiteSpace(createProductDto.Name))
                return ResultPattern.FailResult("Product name cannot be empty");

            if (createProductDto.Price <= 0)
                return ResultPattern.FailResult("Product price cannot be less than or equal to zero");

            if (createProductDto.Image is null)
                return ResultPattern.FailResult("Product image is required");

            var imageUrl = await GlobalConstants.SaveImageAsync(createProductDto.Image, GlobalConstants.productSubPath);
            var vendor = await _productRepository.GetVendorByIdAsync(vendorId);
            if (vendor is null)
                return ResultPattern.FailResult("Vendor not found", 404);

            var product = new Product
            {
                ProductId = Guid.CreateVersion7(),
                ProductName = createProductDto.Name,
                Price = createProductDto.Price,
                Description = createProductDto.Description,
                VendorId = vendor.VendorId,
                VendorName = vendor.VendorName,
                Vendor = vendor,
                ImageUrl = imageUrl,
                CreatedAt = DateTimeOffset.UtcNow
            };
            
            var createdProduct = await _productRepository.CreateProductAsync(product, request);

            var data = MapProductToDto(createdProduct, request);
            return ResultPattern.SuccessResult(data, "Product created successfully");
        }

        public async Task DeleteProductAsync(Guid vendorId, Guid productId)
        {            
            await _productRepository.DeleteProductAsync(vendorId, productId);
        }

        public async Task<ResultPattern> UpdateProductAsync(Guid vendorId, Guid productId, UpdateProductDto updateProduct, HttpRequest request)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product is null)
                return ResultPattern.FailResult("Product not found", 404);

            var vendor = await _productRepository.GetVendorByIdAsync(vendorId);
            if (vendor is null)
                return ResultPattern.FailResult("Vendor not found", 404);

            if (vendor.VendorId != product.VendorId)
                return ResultPattern.FailResult("Vendor does not own this product", 403);

            var imageUrl = updateProduct.Image == null
                ? null
                : await GlobalConstants.SaveImageAsync(updateProduct.Image, GlobalConstants.productSubPath);

            product.UpdateProduct(vendor, vendorId, updateProduct.Name, updateProduct.Description, imageUrl, updateProduct.Price);
            await _productRepository.UpdateProductAsync();

            var data = MapProductToDto(product, request);
            return ResultPattern.SuccessResult(data, "Product updated successfully");
        }


        private static ProductViewDto MapProductToDto (Product product, HttpRequest request)
        {
            return new ProductViewDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                Description = product.Description,
                ImageUrl = GlobalConstants.GetImagetUrl(request, product.ImageUrl),
                VendorId = product.VendorId,
                VendorName = product.VendorName
            };
        }

    }

}
