using DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace EcommerceAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController(IVendorService vendorService) : ControllerBase
    {
        private readonly IVendorService _vendorService = vendorService;

        [HttpPost("create")]
        public async Task<IActionResult> CreateVendorAsync([FromForm] VendorDto vendorDto, HttpRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var vendor = await _vendorService.CreateVendorAsync(vendorDto, request);

            return Ok(vendor);
        }

    }
}
