using System.Threading.Channels;
using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Application.UseCases.Authentication;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Infrastructure.EmailInfrastructure;
using AspNetCoreEcommerce.Shared;
using Microsoft.AspNetCore.Authorization;
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
        AuthServices authServices,
        ILogger<AuthController> logger
            ) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly TokenProvider _tokenService = tokenService;
        private readonly Channel<EmailDto> _emailChannel = emailChannel;
        private readonly ILogger<AuthController> _logger = logger;
        private readonly AuthServices _authServices = authServices;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User(registerDto.Email, registerDto.PhoneNumber, DateTimeOffset.UtcNow);

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                _logger.LogError(result.Errors.ToString());
                return BadRequest();
            }
            
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8604 // Possible null reference argument.
            var email = new EmailDto
            {
                Name = registerDto.Email,
                EmailTo = user.Email,
                Subject = "Confirm your email",
                Body = EmailBodyTemplates.ConfirmEmailBody(token, user.Email)
            };
            await _emailChannel.Writer.WriteAsync(email);

            return Ok("Check your email for confirmation link.");
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto confirmEmailDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _userManager.FindByEmailAsync(confirmEmailDto.Email);
            if (user == null)
                return BadRequest("Invalid Credentials");
            var result = await _userManager.ConfirmEmailAsync(user, confirmEmailDto.Token);
            if (!result.Succeeded)
                return BadRequest();
            await _userManager.AddToRoleAsync(user, "User");
            return Ok("Email confirmed successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return BadRequest("Invalid Credentials");
            var verifyPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!verifyPassword)
                return BadRequest("Invalid Credentials");

            var token = _tokenService.CreateAppUsertoken(user);
            return Ok(new { token });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
                return BadRequest("Invalid Credentials");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8604 // Possible null reference argument.
            var email = new EmailDto
            {
                Name = forgotPasswordDto.Email,
                EmailTo = user.Email,
                Subject = "Reset Password",
                Body = EmailBodyTemplates.ForgotPasswordBody(token, user.Email)
                };
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8601 // Possible null reference assignment.
            await _emailChannel.Writer.WriteAsync(email);
            return Ok("Check your email for the reset password link.");
        }
      
        
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return BadRequest("Invalid Credentials");
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return Ok("Password reset successfully.");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logged out successfully.");
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user == null)
                return BadRequest("Login again");
            if (!user.EmailConfirmed)
                return Unauthorized("Email not confirmed");

            try
            {
                var result = await _authServices.GetUserProfile(user.Id.ToString());
                return result.Success
                    ? Ok(result.Data)
                    : BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Profile not found");
                return BadRequest();
            }
        }
    }

}
