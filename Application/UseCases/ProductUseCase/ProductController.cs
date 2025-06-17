using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Application.UseCases.ProductUseCase
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService productService, UserManager<User> userManager) : ControllerBase
    {
        private readonly IProductService _productService = productService;
        private readonly UserManager<User> _userManager = userManager;

        [Authorize] 
        [HttpPost("create")]
        public async Task<IActionResult> CreateProductAsync([FromForm] CreateProductDto createProduct)
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user is null)
                return Unauthorized();
            if (!user.IsVendor)
                return Forbid("You are not a vendor yet");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var res = await _productService.CreateProductAsync(user.VendorId, createProduct, Request);
            return res.Success
                ? Ok(res.Data)
                : BadRequest(res.Error);
        }

        [Authorize]
        [HttpPut("update/{productID}")]
        public async Task<IActionResult> UpdateProductAsync([FromRoute] Guid productID, [FromForm] UpdateProductDto updateProduct)
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user is null)
                return Unauthorized("Unauthorized");
            if (!user.IsVendor)
                return Forbid("You are not a vendor yet");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var res = await _productService.UpdateProductAsync(user.VendorId, productID, updateProduct, Request);
            return res.Success
                ? Ok(res.Data)
                : BadRequest(res.Error);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductAsync([FromQuery] int page = 1, int pageSize = 50)
        {
            var dto = new PagedProductResponseDto
            {
                PageNumber = page,
                PageSize = pageSize,
                Request = Request
            };
            return Ok(await _productService.GetPagedProductsAsync(dto));
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductByIdAsync([FromRoute] string productId)
        {
            var isValidGuid = Guid.TryParse(productId, out Guid pid);
            if (!isValidGuid)
                return NotFound();
            var res = await _productService.GetProductByIdAsync(pid, Request);
            return res.Success 
                ? Ok(res.Data) 
                : NotFound(res.Error);
        }

        [Authorize]
        [HttpDelete("delete/{productId}")]
        public async Task<IActionResult> DeleteProductAsync([FromRoute] string productId)
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user is null)
                return Unauthorized();
            if (!user.IsVendor)
                return Forbid();

            var isValidGuid = Guid.TryParse(productId, out Guid pid);
            if (!isValidGuid)
                return BadRequest();

            await _productService.DeleteProductAsync(user.VendorId, pid);
            return NoContent();
        }

        [Authorize]
        [HttpPost("reveiw/{pid}")]
        public async Task<IActionResult> ReviewAsync([FromRoute]Guid pid, [FromBody]AddProductReveiwDto dto)
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user is null)
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var res = await _productService.AddProductReviewAsync(user, pid, dto);
            return res.Success ? Ok(res.Data) : NotFound(res.Error);
        }
    }
}
