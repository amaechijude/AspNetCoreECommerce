using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Application.UseCases.Authentication
{
    [Route("")]
    [ApiController]
    public class FormController : ControllerBase
    {
        [DisableCors]
        [HttpPost]
        public IActionResult GetForm([FromBody]Names name)
        {
            Console.WriteLine(name);

            return Ok(new {
                name = name.Name,
                email = name.Email,
                password = name.Password,
            });
        }
    }

    public class Names
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
