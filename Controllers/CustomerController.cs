using System.Security.Claims;
using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(ICustomerService customerService) : ControllerBase
    {
        private readonly ICustomerService _customerService = customerService;        
        
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
    }
}
