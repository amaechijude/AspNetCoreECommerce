using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreEcommerce.Entities
{
    public class Feedback
    {
        [Key]
        public Guid FeedbackId {get; set;}
        public Guid ProdcutId {get; set;}
        public required Product Product {get; set;}
        public Guid CustomerId {get; set;}
        public required Customer Customer {get; set;}
        [Range(1, 5, ErrorMessage = "Rating Must be between 1 and 5")]
        public int Rating {get; set;}
        public required string Comment {get; set;}
        public DateTimeOffset CreatedAt {get; set;}
        public DateTimeOffset UpdatedAt {get; set;}

    }
}