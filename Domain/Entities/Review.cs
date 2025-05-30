using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.Domain.Entities
{
    public class Review
    {
        [Key]
        public Guid Id {get; set;}
        public Guid ProductId { get; set;}
        public required Product Product {get; set;}
        public Guid UserId {get; set;}
        public required User User {get; set;}
        [Range(1, 5, ErrorMessage = "Rating Must be between 1 and 5")]
        public int Rating {get; set;}
        public required string Comment {get; set;}
        public DateTimeOffset CreatedAt {get; set;}
        public DateTimeOffset UpdatedAt {get; set;}

    }
}