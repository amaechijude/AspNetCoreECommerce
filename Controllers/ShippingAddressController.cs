using System.Security.Claims;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingAddressController(IShippingAddressService shippingAddressService) : ControllerBase
    {
        private readonly IShippingAddressService _shippingAddressService = shippingAddressService;

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpPost("add")]
        public async Task<IActionResult> AddShippingAddress([FromBody] ShippingAddressDto shippingAddressDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var validId = Guid.TryParse(userId, out Guid uid);
            if (!validId)
                return BadRequest("Invalid Autentication");

            return Ok(await _shippingAddressService.AddShippingAddressAsync(uid, shippingAddressDto));
        }

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpDelete("delete/{shippingId}")]
        public async Task<IActionResult> DeleteShippingAddress([FromRoute] Guid shippingId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var validId = Guid.TryParse(userId, out Guid uid);
            if (!validId)
                return BadRequest("Invalid Autentication");
            await _shippingAddressService.DeleteShippingAddressAsync(uid, shippingId);
            return Ok();
        }

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpGet("view")]
        public async Task<IActionResult> GetShippingAddressAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var validId = Guid.TryParse(userId, out Guid uid);
            if (!validId)
                return BadRequest("Invalid Autentication");
            return Ok(await _shippingAddressService.GetShippingAddressByCustomerIdAsync(uid));
        }
    }
}
