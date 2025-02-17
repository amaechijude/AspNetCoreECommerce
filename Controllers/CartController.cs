using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Controllers
{
    [ApiController]
    [Authorize(Roles = GlobalConstants.customerRole)]
    [Route("api/[controller]")]
    public class CartController(ICartService cartItemService) : ControllerBase
    {
        private readonly ICartService _cartItemService = cartItemService;

        [HttpPost("add")]
        public async Task<IActionResult> AddToCartAsync([FromBody] CartItemDto cartItem)
        {
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(customerId))
                return BadRequest("Invalid Authentication");

            return Ok( await _cartItemService.ADddToCartAsync(Guid.Parse(customerId), cartItem));
        }

        [HttpPost("remove/{productId}")]
        public async Task<IActionResult> RemoveFromCartAsync([FromRoute] Guid productId)
        {
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(customerId))
                return BadRequest("Invalid Authentication");

            return Ok( await _cartItemService.RemoveFromCartAsync(Guid.Parse(customerId), productId));
        }
    }
}