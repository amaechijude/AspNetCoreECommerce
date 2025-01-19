using DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

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
            try
            {
                return Ok(await _productService.CreateProductAsync(createProduct, request));
            }
            catch (ArgumentException ex) {return BadRequest(ex);}
            catch (KeyNotFoundException ex) {return BadRequest(ex);}
        }
    }
}
