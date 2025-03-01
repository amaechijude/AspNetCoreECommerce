using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCoreEcommerce.Entities
{
    public class Payment
    {
        [Key]
        public Guid PaymentId { get; set; }
        public Guid OrderId {get; set;}
        public required string PaymentReference { get; set; }
        public Order? Order { get; set; }
        public required Guid CustormerId { get; set; }
        public required Customer Customer { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        public string? TransactionReference { get; set; }
        public PaymentStatusEnum PaymentStatus {get; set;}
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset VerificationDate { get; set; }
    }
}
