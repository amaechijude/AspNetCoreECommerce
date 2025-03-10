using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(ICustomerService customerService) : ControllerBase
    {
        private readonly ICustomerService _customerService = customerService;

        [HttpPost("register")]
        public async Task<IActionResult> RegisterCustomer([FromBody] CustomerRegistrationDTO registrationDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _customerService.CreateCustomerAsync(registrationDTO));
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginCustomerAsync(LoginDto login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var res = await _customerService.LoginCustomerAsync(login);
            return res.Success
                ? Ok(res)
                : BadRequest(res);
        }
    }
}
