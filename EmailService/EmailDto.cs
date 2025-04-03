using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.EmailService
{
    public class EmailDto
    {
        [EmailAddress]
        [Required]
        public required string EmailTo {get; set;}
        public required string Name {get; set;}
        public required string Subject {get; set;}
        public required string Body {get; set;}
    }
}