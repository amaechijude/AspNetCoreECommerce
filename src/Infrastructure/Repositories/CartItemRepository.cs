using Data;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class CartItemRepository(ApplicationDbContext context) : ICartItemRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<CartItem> GetOrCreateCartItemAsync(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId)
                ?? throw new CustomerNotFoundException();

            if (customer.CarItems != null)
                return customer.CarItems;

            var cart = new CartItem { Customer = customer };
            _context.CartItems.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }
        public async Task<CartItem> ADddToCartAsync(Customer customer, Product product)
        {
            var userCart = await GetOrCreateCartItemAsync(customer.CustomerID);
            if (userCart.Products.Contains(product))
                throw new ItemAlreadyInCartException();

            userCart.Products.Add(product);
            await _context.SaveChangesAsync();
            return userCart;
        }
        public async Task RemoveFromCartAsync(Customer customer, Product product)
        {
            var userCart = await GetOrCreateCartItemAsync(customer.CustomerID);
            if (!userCart.Products.Contains(product))
                throw new ItemNotFoundException();

            userCart.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }


    public class CustomerNotFoundException() : Exception($"Customer does not exist or is deleted.") { }
    public class ItemAlreadyInCartException() : Exception($"The item is already in the cart.") { }
    public class ItemNotFoundException() : Exception($"The item is not in the cart.") { }
}