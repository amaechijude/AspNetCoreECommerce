using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreEcommerce.Entities
{
    public class AppUser : IdentityUser
    {
        [Key]
        public Guid UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTimeOffset SignupDate { get; set; }
        public DateTimeOffset LastLogin { get; set; }
 
    }
}
