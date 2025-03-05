using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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