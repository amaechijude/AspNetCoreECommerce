using System.Security.Claims;
using AspNetCoreEcommerce.ResultResponse;
using AspNetCoreEcommerce.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderSevice _orderSevice;
        public OrderController(IOrderSevice orderSevice)
        {
            _orderSevice = orderSevice;
        }

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpGet("all")]
        public async Task<IActionResult> GetOrdersByCustomerIdAsync()
        {
            var customerIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var isValid = Guid.TryParse(customerIdString, out Guid customerId);
            if (!isValid)
                return Unauthorized(ResultPattern.FailResult("Invalid Authentication"));

            var res = await _orderSevice.GetOrdersByCustomerIdAsync(customerId);
            return res.Success
                ? Ok(res)
                : BadRequest(res);
        }

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderbyIdAsync([FromRoute] Guid orderId)
        {
            var customerIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var isValid = Guid.TryParse(customerIdString, out Guid customerId);
            if (!isValid)
                return Unauthorized(ResultPattern.FailResult("Invalid Authentication"));

            var res = await _orderSevice.GetOrderByOrderIdAsync(orderId, customerId);
            return res.Success
                ? Ok(res)
                : BadRequest(res);
        }

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpPost("create/{shippingaddressId}")]
        public async Task<IActionResult> CreateOrderAsync([FromRoute] Guid shippingaddressId)
        {
            var customerIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var isValid = Guid.TryParse(customerIdString, out Guid customerId);
            if (!isValid)
                return Unauthorized(ResultPattern.FailResult("Invalid Authentication", 403));

            var res = await _orderSevice.CreateOrderAsync(customerId, shippingaddressId);
            return res.Success
                ? Ok(res)
                : BadRequest(res);
        }
    }
}
