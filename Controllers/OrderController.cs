using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderSevice orderSevice, UserManager<User> userManager) : ControllerBase
    {
        private readonly IOrderSevice _orderSevice = orderSevice;
        private readonly UserManager<User> _userManager = userManager;

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetOrdersByCustomerIdAsync()
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var res = await _orderSevice.GetOrdersByCustomerIdAsync(user.CustomerID);
            return res.Success
                ? Ok(res.Data)
                : BadRequest(res.Error);
        }

        [Authorize]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderbyIdAsync([FromRoute] Guid orderId)
        {
           User? user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var res = await _orderSevice.GetOrderByOrderIdAsync(orderId, user.CustomerID);
            return res.Success
                ? Ok(res.Data)
                : BadRequest(res.Error);
        }

        [Authorize(Roles = GlobalConstants.customerRole)]
        [HttpPost("create/{shippingaddressId}")]
        public async Task<IActionResult> CreateOrderAsync([FromRoute] Guid shippingaddressId)
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var res = await _orderSevice.CreateOrderAsync(user.CustomerID, shippingaddressId);
            return res.Success
                ? Ok(res.Data)
                : BadRequest(res.Error);
        }
    }
}
