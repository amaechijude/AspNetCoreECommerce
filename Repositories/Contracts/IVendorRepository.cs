using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.Repositories.Contracts
{
    public interface IVendorRepository
    {
        public Task<Vendor> SignupVendorAsync(Vendor vendor);
        Task<Vendor> GetVendorByIdAsync(Guid veondorId);
        Task<Vendor> GetVendorByEmailAsync(string email);
        Task SaveChangesAsync();
        Task<bool> CheckExistingVendorEmailAsync(string email);
        Task<bool> CheckExistingVendorNameAsync(string name);
    }
}