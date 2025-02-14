using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreEcommerce.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Controllers
{
    [ApiController]
    [Authorize(Roles = GlobalConstants.customerRole)]
    [Route("api/[controller]")]
    public class CartController(ICartItemService cartItemService) : ControllerBase
    {
        private readonly ICartItemService _cartItemService = cartItemService;

        [HttpPost("add/{productId}")]
        public async Task<IActionResult> AddToCartAsync([FromRoute] Guid productId)
        {
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(customerId))
                return BadRequest("Invalid Authentication");

            return Ok( await _cartItemService.ADddToCartAsync(Guid.Parse(customerId), productId, Request));
        }

        [HttpPost("remove/{productId}")]
        public async Task<IActionResult> RemoveFromCartAsync([FromRoute] Guid productId)
        {
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(customerId))
                return BadRequest("Invalid Authentication");

            return Ok( await _cartItemService.RemoveFromCartAsync(Guid.Parse(customerId), productId, Request));
        }
    }
}