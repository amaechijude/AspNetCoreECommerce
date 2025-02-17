using System.Security.Claims;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Controllers
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
            if (string.IsNullOrEmpty(vendorId))
                return BadRequest("Invalid Authentication");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _productService.CreateProductAsync(Guid.Parse(vendorId), createProduct, Request));
        }

        [Authorize(Roles = GlobalConstants.vendorRole)]
        [HttpPatch("update/{productID}")]
        public async Task<IActionResult> UpdateProductAsync([FromRoute] Guid productID, [FromForm] UpdateProductDto updateProduct)
        {
            var vendorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(vendorId))
                return BadRequest("Invalid Authentication");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _productService.UpdateProductAsync(Guid.Parse(vendorId), productID, updateProduct, Request));
        }

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

        [Authorize(Roles = GlobalConstants.vendorRole)]
        [HttpDelete("delete/{productId}")]
        public async Task<IActionResult> DeleteProductAsync([FromRoute] Guid productId)
        {
            var vendorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(vendorId))
                return BadRequest("Invalid Authentication");

            await _productService.DeleteProductAsync(Guid.Parse(vendorId), productId);
            return Ok(new
            {
                Success = true,
                Message = "Product Deleted Successfully",

            });
        }
    }
}
