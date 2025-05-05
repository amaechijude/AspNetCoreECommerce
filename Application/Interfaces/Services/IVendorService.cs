using AspNetCoreEcommerce.Application.UseCases.VendorUseCase;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Application.Interfaces.Services
{
    public interface IVendorService
    {
        Task<ResultPattern> CreateVendorAsync(User user, CreateVendorDto createVerndor, HttpRequest request);
        Task<ResultPattern> UpdateVendorAsync(Guid vendorId, UpdateVendorDto updateVendor, HttpRequest request);
        Task<ResultPattern> GetVendorByIdAsync(Guid vendorId, HttpRequest request);
        Task<ResultPattern> ActivateVendorAsync(string email, string code, HttpRequest request);
    }
}
