using AspNetCoreEcommerce.Data;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreEcommerce.Repositories.Implementations
{
    public class CartRepository(ApplicationDbContext context) : ICartRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Cart> ADddToCartAsync(Guid customerId, AddToCartDto cartItemDto)
        {
            var pid = Guid.Parse(cartItemDto.ProductId);
            var userCart = await GetOrCreateCartAsync(customerId);
            var product = await GetProductByIdAsync(pid);

            var cartItem = await _context.CartItems
                .Where(ci => ci.ProductId == pid && ci.CartId == userCart.CartId && ci.Product == product)
                .FirstOrDefaultAsync();

            if (cartItem == null)
            {
                var newCartItem = new CartItem
                {
                    CartItemId = Guid.CreateVersion7(),
                    CartId = userCart.CartId,
                    Cart = userCart,
                    UnitPrice = product.Price,
                    TotalPrice = product.Price * cartItemDto.Quantity,
                    ProductId = product.ProductId,
                    Product = product,
                    ProductName = product.ProductName,
                    VendorId = product.VendorId,
                    VendorName = product.VendorName,
                    Quantity = cartItemDto.Quantity,
                    CreatedAt = DateTimeOffset.UtcNow,
                };

                _context.CartItems.Add(newCartItem);
                // userCart.CartItems.Add(newCartItem);
                userCart.CartTotalAmount += product.Price * cartItemDto.Quantity;
                userCart.CartItemsCount += 1;
                await _context.SaveChangesAsync();

                return userCart;

            }
            else
            {
                cartItem.Quantity += cartItemDto.Quantity;
                cartItem.TotalPrice += product.Price * cartItemDto.Quantity;
                cartItem.UpdatedAt = DateTimeOffset.UtcNow;
                userCart.CartTotalAmount += product.Price * cartItemDto.Quantity;
                userCart.UpdatedAt = DateTimeOffset.UtcNow;
                await _context.SaveChangesAsync();
                
                return userCart;
            }
            
        }
        public async Task<CartItem?> RemoveFromCartAsync(Guid customerID, Guid productId)
        {
            var userCart = await GetOrCreateCartAsync(customerID);
            var product = await GetProductByIdAsync(productId);

            var cartItem = await _context.CartItems
                .Where(ci => ci.CartId == userCart.CartId && ci.ProductId == product.ProductId)
                .FirstOrDefaultAsync();

            if (cartItem != null && userCart.CartItems.Contains(cartItem))
            {
                userCart.CartTotalAmount -= cartItem.Product.Price * cartItem.Quantity; 
                userCart.CartItemsCount -= 1;
                userCart.CartItems.Remove(cartItem);
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();

                return cartItem;
            }
            else
            {
                return null;
            }
        }

        public async Task<Cart> GetOrCreateCartAsync(Guid customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId)
                ?? throw new KeyNotFoundException("Customer not found");

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                // .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (cart != null)
                return cart;
            
            var ncart = new Cart
                {
                    CartId = Guid.CreateVersion7(),
                    CustomerId = customer.CustomerID,
                    Customer = customer,
                    CreatedAt = DateTimeOffset.UtcNow,
                };

            _context.Carts.Add(ncart);
            await _context.SaveChangesAsync();
            return ncart;
        }

        private async Task<Product> GetProductByIdAsync(Guid productId)
        {
            var product = await _context.Products.FindAsync(productId)
                ?? throw new ProductNotFoundException("Product does not exist");
            return product;
        }

        // public async Task<Cart> GetCustomerCartAsync(Guid customerId)
        // {
        //     var cart = await _context.Carts
        //         .Include(c => c.CartItems)
        //         .ThenInclude(ci => ci.Product)
        //         .FirstOrDefaultAsync(c => c.CustomerId == customerId)
        //         ?? throw new CustomerNotFoundException("Customer does not exist");
        //     return cart;
        // }
    }


    public class CustomerNotFoundException(string message) : Exception(message);
    public class ItemNotFoundException(string message) : Exception(message);
    public class ProductNotFoundException(string message) : Exception(message);
}
