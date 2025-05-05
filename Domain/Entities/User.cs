using Microsoft.AspNetCore.Identity;

namespace AspNetCoreEcommerce.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName}  {LastName}";
        public Guid? OrderId { get; set; }
        public Order? Order { get; set; }
        public bool IsVendor { get; set; } = false;
        public Guid VendorId { get; set; }
        public Vendor? Vendor { get; set; } = null;
        public Guid CartId { get; set; }
        public Cart? Cart { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; } = [];
        public ICollection<Order> Orders { get; set; } = [];
        public ICollection<Payment> Payments { get; set; } = [];
        public ICollection<ShippingAddress> ShippingAddresses { get; set; } = [];

        public User(string email, string phoneNumber)
        {
            Email = email;
            UserName = email;
            PhoneNumber = phoneNumber;
        }
    }

    public class UserRole : IdentityRole<Guid>
    {
        public UserRole(string name)
        {
            Name = name;
            NormalizedName = name.ToUpper();
        }
    }
}
