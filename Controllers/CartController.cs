using System.Security.Claims;
using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Application.UseCases.CartUseCase;
using AspNetCoreEcommerce.Shared;
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(customerId))
                return BadRequest(ResultPattern.FailResult("Invalid Authentication"));
            var isValidId = Guid.TryParse(customerId, out Guid cId);
            if (!isValidId)
                return BadRequest(ResultPattern.FailResult("Invalid User Id"));

            var isValid = Guid.TryParse(addToCartDto.ProductId, out var _);
            if (!isValid)
                return BadRequest(ResultPattern.FailResult("Invalid Product Id"));

            var data = await _cartService.ADddToCartAsync(cId, addToCartDto);
            return data.Success
                ? Ok(data)
                : BadRequest(data);
        }

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpDelete("remove/{productId}")]
        public async Task<IActionResult> RemoveFromCartAsync([FromRoute] string productId)
        {
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(customerId))
                return BadRequest(ResultPattern.FailResult("Invalid Authentication"));
            var isValidId = Guid.TryParse(customerId, out Guid cId);
            if (!isValidId)
                return BadRequest(ResultPattern.FailResult("Invalid User Id"));

            var isValid = Guid.TryParse(productId, out Guid pid);
            if (!isValid)
                return BadRequest(ResultPattern.FailResult("Invalid Product Id"));

            var data = await _cartService.RemoveFromCartAsync(cId, pid);
            return data.Success
                ? Ok(data)
                : BadRequest(data);
        }

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpGet("view")]
        public async Task<IActionResult> ViewCartAsync()
        {
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(customerId))
                return BadRequest(ResultPattern.FailResult("Invalid Authentication"));
            var isValidId = Guid.TryParse(customerId, out Guid cId);
            if (!isValidId)
                return BadRequest(ResultPattern.FailResult("Invalid User Id"));
            var data =await _cartService.ViewCartAsync(cId);
            return data.Success
                ? Ok(data)
                : BadRequest(data);
        }
    }
}