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
        public async Task<IActionResult> CreateProductAsync([FromForm] CreateProductDto createProduct)
        {
            var vendorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (vendorId is null)
                return BadRequest(vendorId);
        
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _productService.CreateProductAsync(Guid.Parse(vendorId), createProduct, Request));
        }

        // [Authorize(Roles = GlobalConstants.vendorRole)]
        // [HttpPatch("update/{productId}")]
        // public async Task<IActionResult> UpdateProductAsync([FromRoute] Guid productId , [FromForm] UpdateProductDto updateProduct)
        // {
        //     var vendorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //     if (vendorId is null)
        //         return Unauthorized("Invalid token");

        //     if (!ModelState.IsValid)
        //         return BadRequest(ModelState);

        //     var vId = Guid.Parse(vendorId);
        //     var data = await _productService.UpdateProductAsync(productId, vId, updateProduct, Request);
        //     return Ok(data);
        // }

        [HttpGet]
        public async Task<IActionResult> GetAllProductAsync()
        {
            return Ok(await _productService.GetAllProductsAsync(Request));
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductByIdAsync([FromRoute] Guid productId)
        {
            return Ok(await _productService.GetProductByIdAsync(productId, Request));
        }
    }
}
