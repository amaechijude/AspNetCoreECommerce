namespace AspNetCoreEcommerce.Domain.Entities
{
    public class Customer
    {
        public Guid CustomerID { get; set; }
        public required Guid UserId { get; set; }
        public required User User { get; set; }
        public string? Email { get; set; } = string.Empty;
        public required string FirstName { get; set; }
        public required String LastName { get; set; }
        public string FullName => $"{FirstName}  {LastName}";
        public bool IsDeleted { get; set; } = false;
        public Guid CartId {get; set;}
        public Cart? Cart { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; } = [];
        public ICollection<Order> Orders { get; set; } = [];
        public ICollection<Payment> Payments { get; set; } = [];
        public ICollection<ShippingAddress> ShippingAddresses { get; set; } = [];
    }
}
