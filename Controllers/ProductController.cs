using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

        [HttpPost("create")]
        public async Task<IActionResult> CreateProductAsync([FromForm] CreateProductDto createProduct, HttpRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _productService.CreateProductAsync(createProduct, request));
        }
    }
}
