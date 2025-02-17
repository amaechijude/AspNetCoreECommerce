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

        public async Task<Cart> ADddToCartAsync(Guid customerId, CartItemDto cartItemDto)
        {
            var userCart = await GetOrCreateCartAsync(customerId);
            var product = await GetProductByIdAsync(cartItemDto.ProductId);
            var cartItem = await AddToCartItemLogic(userCart, product, cartItemDto.Quantity);

            return cartItem.Cart;
        }
        public async Task<Cart> RemoveFromCartAsync(Guid customerID, Guid productId)
        {
            var userCart = await GetOrCreateCartAsync(customerID);
            var product = await GetProductByIdAsync(productId);

            var cartItem = await _context.CartItems
                .Where(ci => ci.CustomerId == userCart.CustomerId && ci.ProductId == product.ProductId)
                .FirstOrDefaultAsync();

            if (cartItem != null && userCart.CartItems.Contains(cartItem))
            {
                userCart.CartPrice -= (cartItem.Product.Price * cartItem.Quantity); 
                userCart.CartCount -= 1;
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

        private async Task<Cart> GetOrCreateCartAsync(Guid customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId)
                ?? throw new KeyNotFoundException("Customer is disabled");

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerID);

            if (cart is null)
            {
                var userCart = new Cart
                {
                    CartId = customer.CustomerID,
                    CustomerId = customer.CustomerID,
                    Customer = customer,
                    CartItems = []
                };

                _context.Carts.Add(userCart);
                await _context.SaveChangesAsync();

                return userCart;
            }
            return cart;
        }

        private async Task<Product> GetProductByIdAsync(Guid productId)
        {
            var product = await _context.Products.FindAsync(productId)
                ?? throw new ProductNotFoundException("Product does not exist");
            return product;
        }

        private async Task<CartItem> AddToCartItemLogic(Cart userCart, Product product, int Quantity)
        {
            var cartItem = await _context.CartItems
                .Where(ci => ci.CustomerId == userCart.CustomerId && ci.ProductId == product.ProductId)
                .FirstOrDefaultAsync();

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    CartItemId = Guid.CreateVersion7(),
                    CartId = userCart.CartId,
                    Cart = userCart,
                    CustomerId = userCart.CustomerId,
                    Customer = userCart.Customer,
                    ProductId = product.ProductId,
                    Product = product,
                    Quantity = Quantity
                };
                _context.CartItems.Add(cartItem);
                userCart.CartItems.Add(cartItem);
                userCart.CartPrice += product.Price * Quantity;
                userCart.CartCount += 1;
                await _context.SaveChangesAsync();
            }
            else
            {
                cartItem.Quantity += Quantity;
                userCart.CartPrice += product.Price * Quantity;
                await _context.SaveChangesAsync();
            }
            return cartItem;
        }
    }


    public class CustomerNotFoundException(string message) : Exception(message);
    public class ItemNotFoundException(string message) : Exception(message);
    public class ProductNotFoundException(string message) : Exception(message);
}
