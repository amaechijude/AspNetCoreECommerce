using System.Security.Claims;
using AspNetCoreEcommerce.PaymentChannel;
using AspNetCoreEcommerce.ResultResponse;
using AspNetCoreEcommerce.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController(IPaymentService paymentService) : ControllerBase
    {
        private readonly IPaymentService _paymentService = paymentService;

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpPost("initiate/{OrderId}")]
        public async Task<IActionResult> InitiateTransaction(Guid OrderId)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string customerIdString)
                return Unauthorized();

            if (!Guid.TryParse(customerIdString, out Guid customerId))
                return BadRequest("Invalid Customer ID");

            var response = await _paymentService.InitiateTransaction(customerId, OrderId);

            return response switch
            {
                ResultPattern => BadRequest(response),
                InitiateTransactionErrorResponse => BadRequest(response),
                InitiateTransactionSuccessResponse => Ok(response),
                _ => BadRequest("An error occurred while initiating payment")
            };
        }

    }
}
