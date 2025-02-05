
using Microsoft.AspNetCore.Mvc;
using AspNetCoreEcommerce.Services.Contracts;
using AspNetCoreEcommerce.DTOs;

namespace EcommerceAPi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Cart(ICartItemService cartItemService) : ControllerBase
    {
        private readonly ICartItemService _cartItemService = cartItemService;

        [HttpPost("add")]
        public async Task<IActionResult> ADddToCartAsync([FromBody] CartDto cart)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _cartItemService.ADddToCartAsync(cart.CustomerId, cart.ProductId);

            return Ok(new { message = "added to cart" });
        }

    }
}