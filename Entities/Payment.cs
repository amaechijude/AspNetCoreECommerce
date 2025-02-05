using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.Entities
{
    public class Payment
    {
        [Key]
        public Guid PaymentId { get; set; }
        public Order? Order { get; set; }
        public double TotalAmount { get; set; }
        public string? TransactionReference { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset VerificationDate { get; set; }
        public Guid CustormerId { get; set; }
        public Customer? Customer { get; set; }
    }
}
