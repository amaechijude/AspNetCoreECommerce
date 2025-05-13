using System.Security.Claims;
using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Infrastructure.PaymentChannel;
using AspNetCoreEcommerce.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController(IPaymentService paymentService) : ControllerBase
    {
        private readonly IPaymentService _paymentService = paymentService;

        [Authorize]
        [HttpPost("initiate/{OrderId}")]
        public async Task<IActionResult> InitiateTransaction(Guid OrderId)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string customerIdString)
                return Unauthorized();

            if (!Guid.TryParse(customerIdString, out Guid customerId))
                return BadRequest("Invalid User ID");

            var response = await _paymentService.InitiateTransaction(customerId, OrderId);

            return response switch
            {
                InitiateTransactionErrorResponse => BadRequest(response),
                InitiateTransactionSuccessResponse => Ok(response),
                _ => BadRequest("An error occurred while initiating payment")
            };
        }

    }
}
