using Entities;

namespace Repositories
{
    public interface IVendorRepository
    {
        Task<Vendor> CreateVendorAsync(Vendor vendor);
        Task<Vendor> GetVendorByIdAsync(int vendorId);
        Task<Vendor> UpdateVendorIdAsync(int vendorId, Vendor vendor);
        Task DeleteVendorAsync(int vendorId);
    }
}
