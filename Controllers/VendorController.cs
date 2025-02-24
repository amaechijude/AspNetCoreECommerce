using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendorController(IVendorService vendorService) : ControllerBase
    {
        private readonly IVendorService _vendorService = vendorService;


        [HttpPost("register")]
        public async Task<IActionResult> SignupVendorAsync([FromForm] VendorDto vendorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _vendorService.SignupVendorAsync(vendorDto, Request));
        }

        [Authorize(Roles = GlobalConstants.vendorRole)]
        [HttpPatch("update")]
        public async Task<IActionResult> UpdateVendorAsync([FromForm] UpdateVendorDto updateVendor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vendorClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(vendorClaim))
                return BadRequest("Invalid Authentiation");

            var vid = Guid.TryParse(vendorClaim, out Guid VendorId);
            if (!vid)
                return BadRequest("Invalid VendorId");
            return Ok(await _vendorService.UpdateVendorAsync(VendorId, updateVendor, Request));
        }

        [Authorize(Roles = GlobalConstants.vendorRole)]
        [HttpGet("profile")]
        public async Task<IActionResult> GetVendorByIdAsync()
        {
            var vendorClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(vendorClaim))
                return BadRequest("Invalid Authentiation");

            var vid = Guid.TryParse(vendorClaim, out Guid VendorId);
            if (!vid)
                return BadRequest("Invalid VendorId");

            return Ok(await _vendorService.GetVendorByIdAsync(VendorId, Request));
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginVendorAsync([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _vendorService.LoginVendorAsync(loginDto));
        }
    }
}