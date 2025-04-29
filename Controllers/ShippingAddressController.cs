using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Application.UseCases.ShippingAddressUseCase;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingAddressController(IShippingAddressService shippingAddressService, UserManager<User> userManager) : ControllerBase
    {
        private readonly IShippingAddressService _shippingAddressService = shippingAddressService;
        private readonly UserManager<User> _userManager = userManager;

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpPost("add")]
        public async Task<IActionResult> AddShippingAddress([FromBody] ShippingAddressDto shippingAddressDto)
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var res = await _shippingAddressService.AddShippingAddressAsync(user.CustomerID, shippingAddressDto);
            return res.Success
                ? Ok(res.Data)
                : BadRequest(res.Error);
        }

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpDelete("delete/{shippingId}")]
        public async Task<IActionResult> DeleteShippingAddress([FromRoute] string shippingId)
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var isValidGuid = Guid.TryParse(shippingId, out Guid shId);
            if (!isValidGuid)
                return BadRequest();
            var res = await _shippingAddressService.DeleteShippingAddressAsync(user.CustomerID, shId);
            return res.Success
                ? Ok(res.Data)
                : BadRequest(res.Error);
        }

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpGet("view")]
        public async Task<IActionResult> GetShippingAddressAsync()
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var res = await _shippingAddressService.GetShippingAddressByCustomerIdAsync(user.CustomerID);
            return res.Success
                ? Ok(res.Data)
                : BadRequest(res.Error);
        }

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpGet("view/{shippingId}")]
        public async Task<IActionResult> GetShippingAddressByIdAsync([FromRoute] string shippingId)
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var isValidGuid = Guid.TryParse(shippingId, out Guid shId);
            if (!isValidGuid)
                return BadRequest();
            var res = await _shippingAddressService.GetShippingAddressByIdAsync(user.CustomerID, shId);
            return res.Success
                ? Ok(res.Data)
                : BadRequest(res.Error);
        }
    }
}
