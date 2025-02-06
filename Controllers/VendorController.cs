using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController(IVendorService vendorService) : ControllerBase
    {
        private readonly IVendorService _vendorService = vendorService;

        [HttpPost("create")]
        public async Task<IActionResult> CreateVendorAsync([FromForm] VendorDto vendorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var vendor = await _vendorService.CreateVendorAsync(vendorDto, Request);

            return Ok(vendor);
        }

        [HttpGet("get/{vendorid}")]
        public async Task<IActionResult> GetVendorByIdAsync(Guid vendorid)
        {
            return Ok(await _vendorService.GetVendorByIdAsync(vendorid, Request));
        }

        [HttpPost("update/{vendorId}")]
        public async Task<IActionResult> UpdateVendorAsync(Guid vendorId, UpdateVendorDto updateVendor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _vendorService.UpdateVendorByIdAsync(vendorId, updateVendor, Request));
        }

    }
}
