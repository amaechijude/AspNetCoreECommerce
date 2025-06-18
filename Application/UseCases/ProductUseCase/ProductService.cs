using AspNetCoreEcommerce.Application.Interfaces.Repositories;
using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Shared;
using Microsoft.Extensions.Caching.Memory;

namespace AspNetCoreEcommerce.Application.UseCases.ProductUseCase
{
    // Services/ProductService.cs
    public class ProductService(
        IProductRepository productRepository,
        IMemoryCache memoryCache
        ) : IProductService
    {
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IMemoryCache _memoryCache = memoryCache;
        private const int MaxPageSize = 50;

        public async Task<ResultPattern> GetAllProductsAsync(HttpRequest request)
        {
            var data = await _productRepository.GetAllProductsAsync(request);
            return ResultPattern.SuccessResult(data);
        }

        public async Task<ResultPattern> GetProductByIdAsync(Guid productId, HttpRequest request)
        {
            string cacheKey = $"product_{productId}";
            bool InCache = _memoryCache.TryGetValue(cacheKey, out Product? cachedProduct);
            if (InCache && cachedProduct is not null)
            {
                return ResultPattern.SuccessResult(MapProductToDto(cachedProduct, request));
            }
            var product = await _productRepository.GetProductByIdAsync(productId);

            if (product is null)
                return ResultPattern.FailResult("Product not found");

            // store in cache
            _memoryCache.Set(cacheKey, product, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });
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

            if (vendorId != product.VendorId)
                return ResultPattern.FailResult("Vendor does not own this product");

            var imageUrl = updateProduct.Image == null
                ? ""
                : await GlobalConstants.SaveImageAsync(updateProduct.Image, GlobalConstants.productSubPath);

            product.UpdateProduct(updateProduct.Name, updateProduct.Description, imageUrl, updateProduct.Price);
            await _productRepository.UpdateProductAsync();

            var data = MapProductToDto(product, request);
            return ResultPattern.SuccessResult(data);
        }

        public async Task<ResultPattern> AddProductReviewAsync(User user, Guid productId, AddProductReveiwDto reveiwDto)
        {
            var reviewExists = await _productRepository.CheckExistingReviewAsync(productId, user.Id);
            if (reviewExists)
                return ResultPattern.FailResult("Review already exists");

            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product is null)
                return ResultPattern.FailResult("product not found");
            var reveiew = new Review
            {
                Id = productId,
                UserId = user.Id,
                User = user,
                ProductId = product.ProductId,
                Product = product,
                Comment = reveiwDto.Comment,
                Rating = reveiwDto.StarRating,
                CreatedAt = DateTimeOffset.UtcNow
            };
            var cr = await _productRepository.CreateReviewAsync(reveiew);
            if (!cr)
                return ResultPattern.FailResult("failed");
            return ResultPattern.SuccessResult("Added");
        }
        private static ProductViewDto MapProductToDto (Product product, HttpRequest request)
        {
            var img = GlobalConstants.GetImagetUrl(request, product.ImageUrl);
            return new ProductViewDto
            {
                ProductId = product.ProductId,
                ProductName = product.Name,
                Price = product.Price,
                Description = product.Description,
                ImageUrl = img,
                Images = [img, img, img, img],
                VendorId = product.VendorId,
                VendorName = product.VendorName,
                Stock = product.StockQuantity,
                Rating = product.Rating,
                ReveiwCount = product.ReviewCount,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
            };
        }

        public async Task<PagedResponse<ProductViewDto>> GetPagedProductsAsync(PagedProductResponseDto dto)
        {
            int pageNumber = dto.PageNumber < 1 ? 1 : dto.PageNumber;
            int pageSize = pageNumber > MaxPageSize ? MaxPageSize : dto.PageSize;

            var (products, totalCount) = await _productRepository
                .GetPagedProductAsync(pageNumber, pageSize);

            var productDto = products.Select(p => MapProductToDto(p, dto.Request));

            var result = new PagedResponse<ProductViewDto>(
                productDto,
                pageNumber,
                pageSize,
                totalCount
            );
            return result;
        }

        public Task<IEnumerable<ProductViewDto>> GetTrendingProducts(HttpRequest request)
        {
            string cacheKey = "trending_products";
            bool inCache = _memoryCache.TryGetValue(cacheKey, out IEnumerable<ProductViewDto>? cachedProducts);
            if (inCache && cachedProducts is not null)
            {
                return Task.FromResult(cachedProducts);
            }

            var data = _productRepository.GetTrendingProducts(request);
            _memoryCache.Set(cacheKey, data, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });
            return data;
        }
    }

    public record AddProductReveiwDto(int StarRating, string Comment);

}
