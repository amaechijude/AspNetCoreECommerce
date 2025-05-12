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
            User? user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var data = await _cartService.ADddToCartAsync(user.Id, addToCartDto, Request);
            return data.Success
                ? Ok(data.Data)
                : BadRequest(data.Error);
        }

        [Authorize]
        [HttpDelete("remove/{productId}")]
        public async Task<IActionResult> RemoveFromCartAsync([FromRoute] Guid productId)
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var data = await _cartService.RemoveFromCartAsync(user.Id, productId);
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
                : NotFound(data.Error);
        }
    }
}