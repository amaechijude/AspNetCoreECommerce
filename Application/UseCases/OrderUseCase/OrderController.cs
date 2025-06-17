using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEcommerce.Application.UseCases.OrderUseCase
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderSevice orderSevice, UserManager<User> userManager) : ControllerBase
    {
        private readonly IOrderSevice _orderSevice = orderSevice;
        private readonly UserManager<User> _userManager = userManager;

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetOrdersByUserIdAsync()
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var res = await _orderSevice.GetOrdersByUserIdAsync(user.Id);
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

            var res = await _orderSevice.GetOrderByOrderIdAsync(orderId, user.Id);
            return res.Success
                ? Ok(res.Data)
                : BadRequest(res.Error);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrderAsync()
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var res = await _orderSevice.CreateOrderAsync(user.Id);
            return res.Success
                ? Ok(res.Data)
                : BadRequest(res.Error);
        }
    }
}
