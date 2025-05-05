using AspNetCoreEcommerce.Application.Interfaces.Repositories;
using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Application.UseCases.ProductUseCase
{
    // Services/ProductService.cs
    public class ProductService(IProductRepository productRepository) : IProductService
    {
        private readonly IProductRepository _productRepository = productRepository;
        private const int MaxPageSize = 50;

        public async Task<ResultPattern> GetAllProductsAsync(HttpRequest request)
        {
            var data = await _productRepository.GetAllProductsAsync(request);
            return ResultPattern.SuccessResult(data);
        }

        public async Task<ResultPattern> GetProductByIdAsync(Guid productId, HttpRequest request)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);

            if (product is null)
                return ResultPattern.FailResult("Product not found");
            
            return ResultPattern.SuccessResult(MapProductToDto(product, request));
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
                return ResultPattern.FailResult("Vendor not found");

            var product = new Product
            {
                ProductId = Guid.CreateVersion7(),
                Name = createProductDto.Name,
                Price = createProductDto.Price,
                Description = $"{createProductDto.Description}",
                VendorId = vendor.VendorId,
                VendorName = vendor.VendorName,
                Vendor = vendor,
                ImageUrl = imageUrl,
                CreatedAt = DateTimeOffset.UtcNow
            };
            
            var createdProduct = await _productRepository.CreateProductAsync(product, request);

            var data = MapProductToDto(createdProduct, request);
            return ResultPattern.SuccessResult(data);
        }

        public async Task DeleteProductAsync(Guid vendorId, Guid productId)
        {            
            await _productRepository.DeleteProductAsync(vendorId, productId);
        }

        public async Task<ResultPattern> UpdateProductAsync(Guid vendorId, Guid productId, UpdateProductDto updateProduct, HttpRequest request)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product is null)
                return ResultPattern.FailResult("Product not found");

            var vendor = await _productRepository.GetVendorByIdAsync(vendorId);
            if (vendor is null)
                return ResultPattern.FailResult("Vendor not found"      );

            if (vendor.VendorId != product.VendorId)
                return ResultPattern.FailResult("Vendor does not own this product");

            var imageUrl = updateProduct.Image == null
                ? null
                : await GlobalConstants.SaveImageAsync(updateProduct.Image, GlobalConstants.productSubPath);

            product.UpdateProduct(updateProduct.Name, updateProduct.Description, imageUrl, updateProduct.Price);
            await _productRepository.UpdateProductAsync();

            var data = MapProductToDto(product, request);
            return ResultPattern.SuccessResult(data);
        }


        private static ProductViewDto MapProductToDto (Product product, HttpRequest request)
        {
            return new ProductViewDto
            {
                ProductId = product.ProductId,
                ProductName = product.Name,
                Price = product.Price,
                Description = product.Description,
                ImageUrl = GlobalConstants.GetImagetUrl(request, product.ImageUrl),
                VendorId = product.VendorId,
                VendorName = product.VendorName,
                Quantity = product.StockQuantity
            };
        }

        public async Task<PagedResponse<ProductViewDto>> GetPagedProductsAsync(int pageNumber, int pageSize, HttpRequest httpRequest)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageNumber > MaxPageSize ? MaxPageSize : pageSize;

            var (products, totalCount) = await _productRepository
                .GetPagedProductAsync(pageNumber, pageSize);

            var productDto = products.Select(p => MapProductToDto(p, httpRequest));

            return new PagedResponse<ProductViewDto>(
                productDto,
                pageNumber,
                pageSize,
                totalCount
            );
        }
    }

}
