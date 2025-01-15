using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    internal class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public Order? Order { get; set; }
        public double TotalAmount { get; set; }
        public string? TransactionReference { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? VerificationDate { get; set; }
        public int CustormerId { get; set; }
        public Customer? Customer { get; set; }
    }
}
