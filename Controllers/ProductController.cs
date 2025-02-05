using System.Security.Claims;
using AspNetCoreEcommerce;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

        [Authorize(Roles = GlobalConstants.vendorRole)]
        [HttpPost("create")]
        public async Task<IActionResult> CreateProductAsync([FromForm] CreateProductDto createProduct, HttpRequest request)
        {
            var vendorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (vendorId is null)
                return Unauthorized("User is not Authenticated");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _productService.CreateProductAsync(vendorId, createProduct, Request));
        }

        // [Authorize(Roles = GlobalConstants.vendorRole)]
        // [HttpPatch("update")]
        // public async Task<IActionResult> UpdateProductAsync([FromForm] UpdateProductDto updateProduct)
    }
}
