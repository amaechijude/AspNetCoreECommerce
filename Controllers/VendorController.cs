using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Application.UseCases.VendorUseCase;
using AspNetCoreEcommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendorController(IVendorService vendorService, UserManager<User> userManager) : ControllerBase
    {
        private readonly IVendorService _vendorService = vendorService;
        private readonly UserManager<User> _userManager = userManager;

        [HttpPost("register")]
        [Authorize]
        public async Task<IActionResult> SignupVendorAsync([FromForm] CreateVendorDto vendorDto)
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user is null)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _vendorService.CreateVendorAsync(user.Id, vendorDto, Request);
            return result.Success
                ? Ok(result.Data)
                : BadRequest(result.Error);
        }

        [Authorize]
        [HttpPatch("update")]
        public async Task<IActionResult> UpdateVendorAsync([FromForm] UpdateVendorDto updateVendor)
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user is null) return Unauthorized();

            if (!user.IsVendor)
                return BadRequest("User is not a vendor");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _vendorService
                .UpdateVendorAsync(user.VendorID, updateVendor, Request);
            return result.Success
                ? Ok(result.Data)
                : BadRequest(result.Error);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetVendorByIdAsync()
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user is null) return Unauthorized();

            if (!user.IsVendor)
                return BadRequest("User is not a vendor");

            var result = await _vendorService.GetVendorByIdAsync(user.VendorID, Request);
            return result.Success
                ? Ok(result.Data)
                : BadRequest(result.Error);
        }
    }
}
// Compare this snippet from Application/UseCases/VendorUseCase/VendorService.cs: