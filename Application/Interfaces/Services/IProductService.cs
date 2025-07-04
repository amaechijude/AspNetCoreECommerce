﻿using AspNetCoreEcommerce.Application.UseCases.ProductUseCase;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<ResultPattern> GetAllProductsAsync(HttpRequest request);
         Task<PagedResponse<ProductViewDto>> GetPagedProductsAsync(PagedProductResponseDto dto);
        Task<ResultPattern> GetProductByIdAsync(Guid productId, HttpRequest request);
        Task<ResultPattern> CreateProductAsync(Guid vendorId, CreateProductDto productDto, HttpRequest request);
        Task DeleteProductAsync(Guid vendorId, Guid productId);
        Task<ResultPattern> AddProductReviewAsync(
            User user, Guid productId, AddProductReveiwDto reveiwDto
            );
        Task<ResultPattern> UpdateProductAsync(Guid vendorId, Guid productId, UpdateProductDto updateProduct, HttpRequest request);
        Task<IEnumerable<ProductViewDto>> GetTrendingProducts(HttpRequest request);
    }
}
