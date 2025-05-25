using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Application.UseCases.ShippingAddressUseCase;
using AspNetCoreEcommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingAddressController(
        IShippingAddressService shippingAddressService,
        UserManager<User> userManager
        ) : ControllerBase
    {
        private readonly IShippingAddressService _shippingAddressService = shippingAddressService;
        private readonly UserManager<User> _userManager = userManager;

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetShippingAddressByUserIdAsync()
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized("You need to login");

            var res = await _shippingAddressService.GetShippingAddressByUserIdAsync(user.Id);
            return Ok(res);
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddShippingAddress([FromBody] AddShippingAddressDto shippingAddressDto)
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized("You need to login");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var res = await _shippingAddressService.AddShippingAddressAsync(user, shippingAddressDto);
            return res.Success
                ? Ok(res.Data)
                : BadRequest(res.Error);
        }

        [Authorize]
        [HttpDelete("delete/{shId}")]
        public async Task<IActionResult> DeleteShippingAddress([FromRoute] Guid shId)
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();
            var res = await _shippingAddressService.DeleteShippingAddressAsync(user.Id, shId);
            return res.Success
                ? Ok(res.Data)
                : BadRequest(res.Error);
        }

        [Authorize]
        [HttpGet("view/{shippingId}")]
        public async Task<IActionResult> GetShippingAddressByIdAsync([FromRoute] Guid shippingId)
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();
            var res = await _shippingAddressService.GetShippingAddressByIdAsync(user.Id, shippingId);
            return res.Success
                ? Ok(res.Data)
                : BadRequest(res.Error);
        }
    }
}
