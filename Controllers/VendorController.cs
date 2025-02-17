
using System.Security.Claims;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController(IVendorService vendorService) : ControllerBase
    {
        private readonly IVendorService _vendorService = vendorService;

        [HttpPost("register")]
        public async Task<IActionResult> CreateVendorAsync([FromForm] VendorDto vendorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _vendorService.CreateVendorAsync(vendorDto, Request));
        }

        [Authorize(Roles = GlobalConstants.vendorRole)]
        [HttpGet("get")]
        public async Task<IActionResult> GetVendorByIdAsync()
        {
            var vendorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(vendorId))
                return BadRequest("Invalid Authorisation");

            return Ok(await _vendorService.GetVendorByIdAsync(Guid.Parse(vendorId), Request));
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginVendorAsync(LoginDto login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _vendorService.LoginVendorAsync(login));
        }

        [Authorize(Roles = GlobalConstants.vendorRole)]
        [HttpPatch("update")]
        public async Task<IActionResult> UpdateVendorAsync(UpdateVendorDto updateVendor)
        {
            var vendorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(vendorId))
                return BadRequest("Invalid Authentication");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _vendorService.UpdateVendorByIdAsync(Guid.Parse(vendorId), updateVendor, Request));
        }


        // [Authorize(Roles = GlobalConstants.vendorRole)]
        // [HttpDelete("delete")]
        // public async Task<IActionResult> DeleteVendorAsync()
        // {
        //     var vendorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //     if (string.IsNullOrWhiteSpace(vendorId))
        //         return BadRequest("Invalid Authentication");
        //     await _vendorService.DeleteVendorAsync(Guid.Parse(vendorId));
        //     return Ok(new { message = "Vendor deleted successfully" });
        // }
    }
}
