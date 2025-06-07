using AspNetCoreEcommerce.Domain.Entities;

namespace AspNetCoreEcommerce.Application.Interfaces.Repositories
{
    public interface IVendorRepository
    {
        void CreateVendor(Vendor vendor);
        Task<Vendor?> GetVendorByIdAsync(Guid veondorId);
        Task<(IEnumerable<Product> Items, int TotalCount)> GetVendorPagedProductAsync(Guid vendorId, int pageNumber, int pageSize);
        Task<Vendor?> GetVendorByEmailAsync(string email);
        Task<bool> CheckUniqueNameEmail(Guid userId, string email, string name);
        Task SaveChangesAsync();
    }
}