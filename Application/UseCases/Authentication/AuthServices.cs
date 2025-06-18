using AspNetCoreEcommerce.Application.UseCases.ShippingAddressUseCase;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Infrastructure.Data;
using AspNetCoreEcommerce.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AspNetCoreEcommerce.Application.UseCases.Authentication
{
    public class AuthServices(
        UserManager<User> userManager,
        ApplicationDbContext context,
        IMemoryCache memoryCache
        )
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly ApplicationDbContext _context = context;
        private readonly IMemoryCache _memoryCache = memoryCache;

        public async Task<ResultPattern> GetUserProfile(User user)
        {
            var userInfo = MapUserInfo(user);

            var recentOrders = await _context.Orders
                .AsNoTracking()
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
                .AsNoTracking()
                .Where(sh => sh.UserId == user.Id)
                .Select(sh => MapShippingAddress(sh))
                .ToListAsync();

            var reviews = await _context.Reviews
                .AsNoTracking()
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

        public ResultPattern FetchUserAsync(User user)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            return ResultPattern.SuccessResult(new FetchUserDto
            {
                Email = user.Email,
                Name = user.FullName,
                CartCount = user.CartItemsCount,
                IsVendor = user.IsVendor,
            });
#pragma warning restore CS8601 // Possible null reference assignment.
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
