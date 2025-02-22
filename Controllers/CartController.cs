using System.Security.Claims;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController(ICartService cartService) : ControllerBase
    {
        private readonly ICartService _cartService = cartService;

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpPost("add")]
        public async Task<IActionResult> AddToCartAsync([FromBody] AddToCartDto addToCartDto)
        {
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(customerId))
                return BadRequest("Invalid Authentication");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _cartService.ADddToCartAsync(Guid.Parse(customerId), addToCartDto));
        }

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpDelete("remove/{productId}")]
        public async Task<IActionResult> RemoveFromCartAsync([FromRoute] Guid productId)
        {
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(customerId))
                return BadRequest("Invalid Authentication");

            return Ok(await _cartService.RemoveFromCartAsync(Guid.Parse(customerId), productId));
        }

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpGet("view")]
        public async Task<IActionResult> ViewCartAsync()
        {
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(customerId))
                return BadRequest("Invalid Authentication");
            return Ok(await _cartService.ViewCartAsync(Guid.Parse(customerId)));
        }
    }
}