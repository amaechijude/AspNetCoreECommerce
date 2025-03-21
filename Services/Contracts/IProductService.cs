﻿using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.ResultResponse;

namespace AspNetCoreEcommerce.Services.Contracts
{
    public interface IProductService
    {
        Task<ResultPattern> GetAllProductsAsync(HttpRequest request);
        Task<ResultPattern> GetProductByIdAsync(Guid productId, HttpRequest request);
        Task<ResultPattern> CreateProductAsync(Guid vendorId, CreateProductDto productDto, HttpRequest request);
        Task DeleteProductAsync(Guid vendorId, Guid productId);
        Task<ResultPattern> UpdateProductAsync(Guid vendorId, Guid productId, UpdateProductDto updateProduct, HttpRequest request);
    }
}
