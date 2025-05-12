using AspNetCoreEcommerce.Application.Interfaces.Repositories;
using AspNetCoreEcommerce.Application.UseCases.ShippingAddressUseCase;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Infrastructure.Data;
using AspNetCoreEcommerce.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreEcommerce.Application.UseCases.Authentication
{
    public class AuthServices(
        UserManager<User> userManager,
        ApplicationDbContext context
        )
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly ApplicationDbContext _context = context;

        public async Task<ResultPattern> GetUserProfile(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return ResultPattern.FailResult("User Profile Not found");
            var userInfo = MapUserInfo(user);

            var recentOrders = await _context.Orders
                .Where(o => o.UserId == user.Id)
                .OrderDescending()
                .Take(5)
                .Select(o => new OrderSummaryDto
                {
                    Id = o.OrderId,
                    OrderReference = o.OrderRefrence,
                    PlacedAt = o.DateCreated,
                    Status = o.OrderStatus.ToString(),
                    TotalAmount = o.TotalAmountToBePaid
                })
                .ToListAsync();

            var addresses = await _context.ShippingAddresses
                .Where(sh => sh.UserId == user.Id)
                .Select(sh => MapShippingAddress(sh))
                .ToListAsync();

            var reviews = await _context.Reviews
            .Where(r => r.UserId == user.Id)
            .Select(r => new ReviewDto {
                Id = r.Id,
                ProductId = r.ProductId,
                ProductName = r.Product.Name,
                Comment = r.Comment,
                Rating = r.Rating,
                ReviewedAt = r.CreatedAt
            })
            .ToArrayAsync();

            return ResultPattern.SuccessResult(new UserProfileDto
            {
                UserInfo = userInfo,
                OrderSummaries = recentOrders,
                ShippingAddresses = addresses,
                Reviews = reviews
            });
        }

        public async Task<ResultPattern> FetchUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || string.IsNullOrEmpty(user.Email))
                return ResultPattern.FailResult("User not found");
            var data = new FetchUserDto
            {
                Email = user.Email,
                Name = user.FullName,
                CartCount = user.CartItemsCount
            };
            return ResultPattern.SuccessResult(data);
        }

        private static UserInfoDto MapUserInfo(User user)
        {
            return new UserInfoDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Membership = user.MembershipTier(),
                DateJoined = user.DateJoined
            };
        }
        private static ShippingAddressViewDto MapShippingAddress(ShippingAddress shippingAddress)
        {
            return new ShippingAddressViewDto
            {
                ShippingAddressId = shippingAddress.ShippingAddressId,
                UserId = shippingAddress.UserId,
                UserName = $"{shippingAddress.FirstName} {shippingAddress.LastName}",
                ShippingAddressName = $"{shippingAddress.FirstName} {shippingAddress.LastName}",
                ShippingAddressPhone = shippingAddress.PhoneNumber,
                AddressOne = shippingAddress.AddressLine1,
                SecondAddress = shippingAddress.AddressLine2,
                City = shippingAddress.City,
                State = shippingAddress.State,
                Country = shippingAddress.Country,
                PostalCode = shippingAddress.ZipCode,
            };
        }
    }
}
