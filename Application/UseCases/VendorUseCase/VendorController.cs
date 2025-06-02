using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Application.UseCases.VendorUseCase
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendorController(
        IVendorService vendorService,
        UserManager<User> userManager
        ) : ControllerBase
    {
        private readonly IVendorService _vendorService = vendorService;
        private readonly UserManager<User> _userManager = userManager;

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> SignupVendorAsync([FromForm] CreateVendorDto vendorDto)
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user is null || !user.EmailConfirmed) 
                return Forbid("Confirm user email");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _vendorService.CreateVendorAsync(user, vendorDto, Request);
            return result.Success
                ? Ok(result.Data)
                : BadRequest(result.Error);
        }


        [Authorize]
        [HttpPost]
       public async Task<IActionResult> ActivateVendor([FromBody] ActivateVendorDto activateVendor)
        {
            var res = await _vendorService.ActivateVendorAsync(activateVendor, Request);
            return res.Success
                ? Ok(res.Data) 
                : BadRequest(res.Error); 
        }

        [Authorize]
        [HttpPatch("update")]
        public async Task<IActionResult> UpdateVendorAsync([FromForm] UpdateVendorDto updateVendor)
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user is null) return Unauthorized();

            if (!IsVendor(user))
                return BadRequest("User is not a vendor");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _vendorService
                .UpdateVendorAsync(user.VendorId, updateVendor, Request);
            return result.Success
                ? Ok(result.Data)
                : BadRequest(result.Error);
        }

        [Authorize]
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetVendorByIdAsync()
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user is null) return Unauthorized();

          var result = await _vendorService.GetVendorByIdAsync(user, Request);
            return result.Success
                ? Ok(result.Data)
                : BadRequest(result.Error);
        }

        private static bool IsVendor(User user)
        {
            if (user.Vendor is null || !user.IsVendor)
                return false;
            return true;
        }
    }
}
// Compare this snippet from Application/UseCases/VendorUseCase/VendorService.cs: