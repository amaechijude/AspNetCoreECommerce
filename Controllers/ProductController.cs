using System.Security.Claims;
using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Application.UseCases.ProductUseCase;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Shared;
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vendorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(vendorId))
                return BadRequest(ResultPattern.FailResult("Invalid Authentication", 400));

            var isValidGuid = Guid.TryParse(vendorId, out Guid vid);
            if (!isValidGuid)
                return BadRequest(ResultPattern.FailResult("Invalid Vendor Id", 400));
            var res = await _productService.CreateProductAsync(vid, createProduct, Request);
            return res.Success
                ? Ok(res)
                : BadRequest(res);
        }

        [Authorize(Roles = GlobalConstants.vendorRole)]
        [HttpPatch("update/{productID}")]
        public async Task<IActionResult> UpdateProductAsync([FromRoute] Guid productID, [FromForm] UpdateProductDto updateProduct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var vendorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(vendorId))
                return BadRequest(ResultPattern.FailResult("Invalid Authentication", 400));

            var isValidGuid = Guid.TryParse(vendorId, out Guid vid);
            if (!isValidGuid)
                return BadRequest(ResultPattern.FailResult("Invalid Vendor Id", 400));
            
            var res = await _productService.UpdateProductAsync(vid, productID, updateProduct, Request);
            return res.Success
                ? Ok(res)
                : BadRequest(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductAsync()
        {
            return Ok(await _productService.GetAllProductsAsync(Request));
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductByIdAsync([FromRoute] string productId)
        {
            var isValidGuid = Guid.TryParse(productId, out Guid pid);
            if (!isValidGuid)
                return BadRequest(ResultPattern.FailResult("Invalid Product Id", 400));
            var res = await _productService.GetProductByIdAsync(pid, Request);
            return res.Success 
                ? Ok(res) 
                : NotFound(res);
        }

        [Authorize(Roles = GlobalConstants.vendorRole)]
        [HttpDelete("delete/{productId}")]
        public async Task<IActionResult> DeleteProductAsync([FromRoute] string productId)
        {
            var vendorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(vendorId))
                return BadRequest(ResultPattern.FailResult("Invalid Authentication", 400));

            var isValidGuid = Guid.TryParse(productId, out Guid pid);
            if (!isValidGuid)
                return BadRequest(ResultPattern.FailResult("Invalid Product Id", 400));

            await _productService.DeleteProductAsync(Guid.Parse(vendorId), pid);
            return Ok(ResultPattern.SuccessResult("", "Product deleted successfully"));
        }
    }
}
