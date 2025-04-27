using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using AspNetCoreEcommerce.EmailService;
using AspNetCoreEcommerce.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Authentication
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        TokenProvider tokenService,
        Channel<EmailDto> emailChannel
            ) : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly TokenProvider _tokenService = tokenService;
        private readonly Channel<EmailDto> _emailChannel = emailChannel;

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

            return Ok(user);
        }

        private static AppUser AppUserBuilder(RegisterDto registerDto)
        {
            return new AppUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber
            };
        }
    }
}
