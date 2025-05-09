using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.Infrastructure.EmailInfrastructure
{
    public class EmailDto
    {
        public required string EmailTo {get; set;}
        public required string Subject {get; set;}
        public required string Body {get; set;}
        public string? Name {get; set;}
    }
}