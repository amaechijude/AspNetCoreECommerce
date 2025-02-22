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
            var userCart = await GetOrCreateCartAsync(customerId);
            var product = await GetProductByIdAsync(cartItemDto.ProductId);

            var cartItem = await _context.CartItems
               .Where(ci => ci.CartId == userCart.CartId && ci.ProductId == product.ProductId)
               .FirstOrDefaultAsync();

            if (cartItem == null)
            {
                var newCartItem = new CartItem
                {
                    CartItemId = Guid.CreateVersion7(),
                    CartId = userCart.CartId,
                    Cart = userCart,
                    ProductId = product.ProductId,
                    Product = product,
                    Quantity = cartItemDto.Quantity
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
                userCart.CartTotalAmount += product.Price * cartItemDto.Quantity;
                await _context.SaveChangesAsync();
                return cartItem.Cart;
            }
            
        }
        public async Task<Cart> RemoveFromCartAsync(Guid customerID, Guid productId)
        {
            var userCart = await GetOrCreateCartAsync(customerID);
            var product = await GetProductByIdAsync(productId);

            var cartItem = await _context.CartItems
                .Where(ci => ci.CartId == userCart.CartId && ci.ProductId == product.ProductId)
                .FirstOrDefaultAsync();

            if (cartItem != null && userCart.CartItems.Contains(cartItem))
            {
                userCart.CartTotalAmount -= (cartItem.Product.Price * cartItem.Quantity); 
                userCart.CartItemsCount -= 1;
                userCart.CartItems.Remove(cartItem);
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();

                return userCart;
            }
            else
            {
                return userCart;
            }
        }

        public async Task<Cart> GetOrCreateCartAsync(Guid customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId)
                ?? throw new KeyNotFoundException("Customer is disabled");

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.CartId ==customer.CustomerID);

            if (cart is null)
            {
                cart = new Cart
                {
                    CartId = customer.CustomerID,
                    CustomerId = customer.CustomerID,
                    Customer = customer
                };

                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();

                return cart;
            }
            return cart;
        }

        private async Task<Product> GetProductByIdAsync(Guid productId)
        {
            var product = await _context.Products.FindAsync(productId)
                ?? throw new ProductNotFoundException("Product does not exist");
            return product;
        }

        public async Task<Cart> GetCustomerCartAsync(Guid customerId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId)
                ?? throw new CustomerNotFoundException("Customer does not exist");
            return cart;
        }
    }


    public class CustomerNotFoundException(string message) : Exception(message);
    public class ItemNotFoundException(string message) : Exception(message);
    public class ProductNotFoundException(string message) : Exception(message);
}
