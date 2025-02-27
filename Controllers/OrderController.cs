using System.Security.Claims;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderSevice orderSevice) : ControllerBase
    {
        private readonly IOrderSevice _orderSevice = orderSevice;

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpGet("all")]
        public async Task<IActionResult> GetOrdersByCustomerIdAsync()
        {
            var customerIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(customerIdString))
                return Unauthorized("Invalid Authentication");

            var isValid = Guid.TryParse(customerIdString, out Guid customerId);
            if (!isValid)
                return Unauthorized("Invalid Authentication");

            return Ok(await _orderSevice.GetOrdersByCustomerIdAsync(customerId));
        }

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderbyIdAsync([FromRoute] Guid orderId)
        {
            var customerIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(customerIdString))
                return Unauthorized("Invalid Authentication");

            var isValid = Guid.TryParse(customerIdString, out Guid customerId);
            if (!isValid)
                return Unauthorized("Invalid Authentication");

            return Ok(await _orderSevice.GetOrderByOrderIdAsync(orderId, customerId));
        }

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpPost("create/{shippingaddressId}")]
        public async Task<IActionResult> CreateOrderAsync([FromRoute] Guid shippingaddressId)
        {
            var customerIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(customerIdString))
                return Unauthorized("Invalid Authentication");

            var isValid = Guid.TryParse(customerIdString, out Guid customerId);
            if (!isValid)
                return Unauthorized("Invalid Authentication");

            return Ok(await _orderSevice.CreateOrderAsync(customerId, shippingaddressId));
        }
    }
}
