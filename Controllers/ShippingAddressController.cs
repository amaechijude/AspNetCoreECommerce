using System.Security.Claims;
using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Application.UseCases.ShippingAddressUseCase;
using AspNetCoreEcommerce.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingAddressController : ControllerBase
    {
        private readonly IShippingAddressService _shippingAddressService;
        public ShippingAddressController(IShippingAddressService shippingAddressService)
        {
            _shippingAddressService = shippingAddressService;
        }

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpPost("add")]
        public async Task<IActionResult> AddShippingAddress([FromBody] ShippingAddressDto shippingAddressDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResultPattern.BadModelState(ModelState));

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var validId = Guid.TryParse(userId, out Guid uid);
            if (!validId)
                return BadRequest(ResultPattern.FailResult("Invalid Autentication"));

            var res = await _shippingAddressService.AddShippingAddressAsync(uid, shippingAddressDto);
            return res.Success
                ? Ok(res)
                : BadRequest(res);
        }

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpDelete("delete/{shippingId}")]
        public async Task<IActionResult> DeleteShippingAddress([FromRoute] string shippingId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var validId = Guid.TryParse(userId, out Guid uid);
            if (!validId)
                return BadRequest(ResultPattern.FailResult("Invalid Autentication"));

            var isValidGuid = Guid.TryParse(shippingId, out Guid shId);
            if (!isValidGuid)
                return BadRequest(ResultPattern.FailResult("Invalid Shipping Address Id"));
            var res = await _shippingAddressService.DeleteShippingAddressAsync(uid, shId);
            return res.Success
                ? Ok(res)
                : BadRequest(res);
        }

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpGet("view")]
        public async Task<IActionResult> GetShippingAddressAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var validId = Guid.TryParse(userId, out Guid uid);
            if (!validId)
                return BadRequest(ResultPattern.FailResult("Invalid Autentication"));

            var res = await _shippingAddressService.GetShippingAddressByCustomerIdAsync(uid);
            return res.Success
                ? Ok(res)
                : BadRequest(res);
        }

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpGet("view/{shippingId}")]
        public async Task<IActionResult> GetShippingAddressByIdAsync([FromRoute] string shippingId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var validId = Guid.TryParse(userId, out Guid uid);
            if (!validId)
                return BadRequest(ResultPattern.FailResult("Invalid Autentication"));
            var isValidGuid = Guid.TryParse(shippingId, out Guid shId);
            if (!isValidGuid)
                return BadRequest(ResultPattern.FailResult("Invalid Shipping Address Id"));
            var res = await _shippingAddressService.GetShippingAddressByIdAsync(uid, shId);
            return res.Success
                ? Ok(res)
                : BadRequest(res);
        }
    }
}
