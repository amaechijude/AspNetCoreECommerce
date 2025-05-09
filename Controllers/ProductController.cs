﻿using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Application.UseCases.ProductUseCase;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService productService, UserManager<User> userManager) : ControllerBase
    {
        private readonly IProductService _productService = productService;
        private readonly UserManager<User> _userManager = userManager;

        [Authorize(Roles = "Vendor")] 
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

        [Authorize(Roles = "Vendor")]
        [HttpPatch("update/{productID}")]
        public async Task<IActionResult> UpdateProductAsync([FromRoute] Guid productID, [FromForm] UpdateProductDto updateProduct)
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user is null)
                return Unauthorized();
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
        public async Task<IActionResult> GetAllProductAsync([FromQuery] int pageNumber = 1, int pageSize = 50)
        {
            return Ok(await _productService.GetPagedProductsAsync(pageNumber, pageSize, Request));
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

        [Authorize(Roles = "Vendor")]
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
        public async Task<IActionResult> ReviewAsync([FromQuery] Guid pid, [FromBody]AddProductReveiwDto dto)
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
