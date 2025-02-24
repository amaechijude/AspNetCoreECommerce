using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreEcommerce.Entities
{
    public class OrderItem
    {
        [Key]
        public Guid OrderItemID {get; set;}
        public Guid OrderId {get; set;}
        public Order? Order {get; set;}
        public Guid ProductId {get; set;}
        public required Product Product {get; set;}
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice {get; set;}
        public int Quantity {get; set;}
        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount {get; set;}
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice {get; set;}
        

    }
}