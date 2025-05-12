using AspNetCoreEcommerce.Application.Interfaces.Repositories;
using AspNetCoreEcommerce.Application.UseCases.CartUseCase;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreEcommerce.Infrastructure.Repositories
{
    public class CartRepository(
        ApplicationDbContext context,
        UserManager<User> userManager,
        ILogger<CartRepository> logger
        ) : ICartRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<User> _userManager = userManager;
        private readonly ILogger<CartRepository> _logger = logger;

        public async Task<bool> ADddToCartAsync(Guid userId, Guid productId, int quantity)
        {
            var userCart = await GetOrCreateCartAsync(userId);
            var product = await _context.Products.FindAsync(productId);
            if (product is null)
                return false;

            var cartItem = await _context.CartItems
                .Where(ci => ci.ProductId == productId && ci.CartId == userCart.CartId)
                .FirstOrDefaultAsync();

            if (cartItem is null)
            {
                cartItem = new CartItem
                {
                    CartItemId = Guid.CreateVersion7(),
                    CartId = userCart.CartId,
                    Cart = userCart,
                    ProductId = product.ProductId,
                    Product = product,
                    VendorId = product.VendorId,
                    VendorName = product.VendorName,
                    ProductName = product.Name,
                    Quantity = quantity,
                    UnitPrice = product.Price,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow,
                };
                userCart.AddCartItem(cartItem);
            }else{
                cartItem.UpdateCartItem(quantity);
            }

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RemoveFromCartAsync(Guid userId, Guid productId)
        {
            var userCart = await GetOrCreateCartAsync(userId);
            var cartItem = await _context.CartItems
                .Where(ci => ci.CartId == userCart.CartId && ci.ProductId == productId)
                .FirstOrDefaultAsync();
            if (cartItem is null)
                return false;
            userCart.RemoveCartItem(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<Cart> GetOrCreateCartAsync(Guid userId)
        {
            var cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart is not null)
                return cart;
            var user = await _userManager.FindByIdAsync(userId.ToString())
                ?? throw new NullCartException("User does not exist");
            var ncart = new Cart
            {
                CartId = Guid.CreateVersion7(),
                UserId = userId,
                User = user,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            _context.Carts.Add(ncart);
            await SaveChangesAsync();
            return ncart;
        }
        public async Task<Cart> ViewCartAsync(Guid userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId)
                ?? await GetOrCreateCartAsync(userId);
            return cart;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
    public class NullCartException(string message) : Exception(message);
}
