using Microsoft.AspNetCore.Identity;

namespace AspNetCoreEcommerce.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public Guid CustomerID { get; set; } = Guid.Empty;
        public Customer? Customer { get; set; }
        public bool IsVendor { get; set; } = false;
        public Guid VendorID { get; set; }
        public Vendor? Vendor { get; set; }
    }

    public class UserRole : IdentityRole<Guid>
    {

    }
}
