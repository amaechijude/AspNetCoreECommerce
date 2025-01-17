using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class CustomerRegistrationDTO
    {
        [EmailAddress]
        [Required]
        public string? CustomerEmail { get; set; }
        [Required]
        [PasswordPropertyText]
        public string? Password { get; set; }
        [Required]
        public string? CustomerName { get; set; }
        [Required]
        public string? CustomerPhone { get; set; }
    }
}
