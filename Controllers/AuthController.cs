using System.Threading.Channels;
using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Application.UseCases.Authentication;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Infrastructure.EmailInfrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        TokenProvider tokenService,
        Channel<EmailDto> emailChannel,
        ICustomerService customerService
            ) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly TokenProvider _tokenService = tokenService;
        private readonly Channel<EmailDto> _emailChannel = emailChannel;
        private readonly ICustomerService _customerService = customerService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = AppUserBuilder(registerDto);
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);
            user.EmailConfirmed = true;

            _ = await _userManager.AddToRoleAsync(user, "Customer");
            await _customerService.CreateCustomerAsync(user, registerDto.FirstName, registerDto.LastName);

            return Ok(user.Id);
        }

        private static User AppUserBuilder(RegisterDto registerDto)
        {
            return new User
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
            };
        }
    }
}
