using System.Security.Claims;
using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Infrastructure.PaymentChannel;
using AspNetCoreEcommerce.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Application.UseCases.PaymentUseCase
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController(
        IPaymentService paymentService,
        UserManager<User> userManager
        ) : ControllerBase
    {
        private readonly IPaymentService _paymentService = paymentService;
        private readonly UserManager<User> _userManager = userManager;

        [Authorize]
        [HttpPost("initiate")]
        public async Task<IActionResult> InitiateTransaction([FromBody] PaymentDto pto)
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _paymentService.InitiateTransaction(user, pto);

            return response switch
            {
                InitiateTransactionErrorResponse => BadRequest(response),
                InitiateTransactionSuccessResponse => Ok(response),
                _ => BadRequest("An error occurred while initiating payment")
            };
        }

    }
}
