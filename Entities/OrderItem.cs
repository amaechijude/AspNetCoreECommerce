// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// namespace AspNetCoreEcommerce.Entities
// {
//     public class OrderItem
//     {
//         public Guid OrderItemID {get; set;}
//         public Guid ProductId {get; set;}
//         public double ProductPrice {get; set;}
//         public required Product Product {get; set;}
//         public int Quantity {get; set;}
//         public double TotalPrice => ProductPrice * Quantity;

//     }
// }