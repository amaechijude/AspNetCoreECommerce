using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public double UnitPrice {get; set;}
        public int Quantity {get; set;}
        public double Discount {get; set;}
        public double TotalPrice {get; set;}
        

    }
}