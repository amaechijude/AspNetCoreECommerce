using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
            var CustomerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (CustomerId == null)
                return Unauthorized();

            bool isValidGuid = Guid.TryParse(CustomerId, out Guid customerId);
            if (!isValidGuid)
                return BadRequest();
            var response = await _paymentService.InitiateTransaction(customerId, OrderId);

            return response != null
                ? Ok(response)
                : BadRequest("An error occurred while initiating payment");
        }

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpGet("verify/{paymentReference}")]
        public async Task<IActionResult> VerifyTransaction(string paymentReference)
        {
            var response = await _paymentService.VerifyTransaction(paymentReference);
            return response != null
                ? Ok(response)
                : BadRequest("An error occurred while verifying payment");
        }
        
    }
}
