using AspNetCoreEcommerce.Application.UseCases.ProductUseCase;
using AspNetCoreEcommerce.Application.UseCases.VendorUseCase;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Application.Interfaces.Services
{
    public interface IVendorService
    {
        Task<ResultPattern> CreateVendorAsync(User user, CreateVendorDto createVerndor, HttpRequest request);
        Task<ResultPattern> UpdateVendorAsync(Guid vendorId, UpdateVendorDto updateVendor, HttpRequest request);
        Task<ResultPattern> GetVendorByIdAsync(User user, HttpRequest request);
        Task<PagedResponse<ProductViewDto>> GetVendorPagedProductsAsync(PagedResponseDto pagedResponse);
        Task<ResultPattern> ActivateVendorAsync(ActivateVendorDto dto, HttpRequest request);
    }
}
