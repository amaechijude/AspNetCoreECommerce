using System.Security.Claims;
using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Application.UseCases.CartUseCase;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController(ICartService cartService, UserManager<User> userManager) : ControllerBase
    {
        private readonly ICartService _cartService = cartService;
        private readonly UserManager<User> _userManager = userManager;

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddToCartAsync([FromBody] AddToCartDto addToCartDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            User? user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var isValid = Guid.TryParse(addToCartDto.ProductId, out var _);
            if (!isValid)
                return BadRequest("Invalid Product Id");
            Guid id = user.Id;
            var data = await _cartService.ADddToCartAsync(id ,addToCartDto, Request);
            return data.Success
                ? Ok(data.Data)
                : BadRequest(data.Error);
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
                ? Ok(data.Data)
                : BadRequest(data.Error);
        }

        [Authorize]
        [HttpGet("view")]
        public async Task<IActionResult> ViewCartAsync()
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();
            var data = await _cartService.ViewCartAsync(user.Id, Request);
            return data.Success
                ? Ok(data.Data)
                : BadRequest(data.Error);
        }
    }
}