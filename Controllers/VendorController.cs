using System.Security.Claims;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.ResultResponse;
using AspNetCoreEcommerce.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendorController : ControllerBase
    {
        private readonly IVendorService _vendorService;
        public VendorController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> SignupVendorAsync([FromForm] VendorDto vendorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var res = await _vendorService.SignupVendorAsync(vendorDto, Request);
            return res.Success
                ? Ok(res)
                : BadRequest(res);
        }

        [Authorize(Roles = GlobalConstants.vendorRole)]
        [HttpPatch("update")]
        public async Task<IActionResult> UpdateVendorAsync([FromForm] UpdateVendorDto updateVendor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vendorClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var vid = Guid.TryParse(vendorClaim, out Guid VendorId);
            if (!vid)
                return BadRequest(ResultPattern.FailResult("Invalid VendorId"));

            var re = await _vendorService.UpdateVendorAsync(VendorId, updateVendor, Request);
            return re.Success
                ? Ok(re)
                : BadRequest(re);
        }

        [Authorize(Roles = GlobalConstants.vendorRole)]
        [HttpGet("profile")]
        public async Task<IActionResult> GetVendorByIdAsync()
        {
            var vendorClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var vid = Guid.TryParse(vendorClaim, out Guid VendorId);
            if (!vid)
                return BadRequest(ResultPattern.FailResult("Invalid VendorId"));

            var res = await _vendorService.GetVendorByIdAsync(VendorId, Request);
            return res.Success
                ? Ok(res)
                : BadRequest(res);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginVendorAsync([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResultPattern.BadModelState(ModelState));

            var res = await _vendorService.LoginVendorAsync(loginDto);
            return res.Success
                ? Ok(res)
                : BadRequest(res);
        }

        [HttpPost("activate")]
        public async Task<IActionResult> ActivateVendorAsync([FromBody] VerificationRequest activateVendor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResultPattern.BadModelState(ModelState));

            var res = await _vendorService
                .ActivateVendorAsync(activateVendor.Email, activateVendor.Code, Request);
            return res.Success
                ? Ok(res)
                : BadRequest(res);
        }
    }
}