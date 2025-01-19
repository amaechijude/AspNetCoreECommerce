
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.DataProtection.Repositories;
using AspNetCoreEcommerce.Services.Contracts;
using AspNetCoreEcommerce.Respositories.Implementations;
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

            try
            {
                await _cartItemService.ADddToCartAsync(cart.CustomerId, cart.ProductId);

                return Ok(new {message = "added to cart"});
            }
            
            catch (ProductNotFoundException ex) { return BadRequest(ex);}
            catch (ItemAlreadyInCartException ex) { return BadRequest(ex);}
            catch (CustomerNotFoundException ex) { return BadRequest(ex);}
        }

    }
}