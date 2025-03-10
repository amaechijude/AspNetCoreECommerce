using System.Security.Claims;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.ResultResponse;
using AspNetCoreEcommerce.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
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

            var data = await _customerService.CreateCustomerAsync(registrationDTO);
            return data.Success
                ? Ok(data)
                : BadRequest(data);
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
        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpGet("profile")]
        public async Task<IActionResult> GetCustomerProfile()
        {
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(customerId))
                return BadRequest(ResultPattern.FailResult("Invalid Authentication"));

            var isValidGuid = Guid.TryParse(customerId, out Guid customerIdGuid);
            if (!isValidGuid)
                return BadRequest(ResultPattern.FailResult("Invalid Authentication"));

            var res = await _customerService.GetCustomerByIdAsync(customerIdGuid);
            return res.Success
                ? Ok(res)
                : BadRequest(res);
        }

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCustomerProfile([FromBody] UpdateCustomerDto customer)
        {
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(customerId))
                return BadRequest(ResultPattern.FailResult("Invalid Authentication"));
            var isValidGuid = Guid.TryParse(customerId, out Guid customerIdGuid);
            if (!isValidGuid)
                return BadRequest(ResultPattern.FailResult("Invalid Authentication"));
            var res = await _customerService.UpdateCustomerAsync(customerIdGuid, customer);
            return res.Success
                ? Ok(res)
                : BadRequest(res);
        }

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCustomerProfile()
        {
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(customerId))
                return BadRequest(ResultPattern.FailResult("Invalid Authentication"));
            var isValidGuid = Guid.TryParse(customerId, out Guid customerIdGuid);
            if (!isValidGuid)
                return BadRequest(ResultPattern.FailResult("Invalid Authentication"));
            var res = await _customerService.DeleteCustomerAsync(customerIdGuid);
            return res.Success
                ? Ok(res)
                : BadRequest(res);
        }
        [HttpPost("verify")]
        public async Task<IActionResult> VerifyCodeAsync(VerificationRequest verification)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResultPattern.BadModelState(ModelState));

            var res = await _customerService.VerifyCodeAsync(verification);
            return res.Success
                ? Ok(res)
                : BadRequest(res);
        }
    }
}
