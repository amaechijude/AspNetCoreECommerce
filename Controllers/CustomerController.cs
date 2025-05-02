using System.Security.Claims;
using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService customerService) : ControllerBase
    {
        private readonly IUserService _customerService = customerService;        
        
        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(customerId))
                return BadRequest(ResultPattern.FailResult("Invalid Authentication"));

            var isValidGuid = Guid.TryParse(customerId, out Guid customerIdGuid);
            if (!isValidGuid)
                return BadRequest(ResultPattern.FailResult("Invalid Authentication"));

            var res = await _customerService.GetUserByIdAsync(customerIdGuid);
            return res.Success
                ? Ok(res)
                : BadRequest(res);
        }


        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUserProfile()
        {
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(customerId))
                return BadRequest(ResultPattern.FailResult("Invalid Authentication"));
            var isValidGuid = Guid.TryParse(customerId, out Guid customerIdGuid);
            if (!isValidGuid)
                return BadRequest(ResultPattern.FailResult("Invalid Authentication"));
            var res = await _customerService.DeleteUserAsync(customerIdGuid);
            return res.Success
                ? Ok(res)
                : BadRequest(res);
        }
    }
}
