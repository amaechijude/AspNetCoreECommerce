using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.Respositories.Contracts
{
    public interface IVendorRepository
    {
        Task<Vendor> CreateVendorAsync(Vendor vendor, HttpRequest request);
        Task<Vendor> GetVendorByIdAsync(Guid vendorId);
        Task<Vendor?> GetVendorByEmailAsync(string vendorEmail);
        Task SaveUpdateVendorAsync();
        Task DeleteVendorAsync(Vendor vendor);
    }
}
