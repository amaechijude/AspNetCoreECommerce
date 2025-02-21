using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.Repositories.Contracts
{
    public interface IVendorRepository
    {
        Task<Vendor> CreateVendorAsync(Vendor vendor);
        Task<Vendor> GetVendorByIdAsync(Guid vendorId);
        Task<Vendor> GetVendorByEmailAsync(string vendorEmail);
        Task SaveUpdateVendorAsync();
        // Task DeleteVendorAsync(Vendor vendor);
    }
}
