using AspNetCoreEcommerce.Data;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Respositories.Contracts;

namespace AspNetCoreEcommerce.Respositories.Implementations
{
    public class CartItemRepository(ApplicationDbContext context) : ICartItemRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<CartItem> ADddToCartAsync(Guid customerID, Guid productId)
        {
            var userCart = await GetOrCreateCartItemAsync(customerID);

            var product = await _context.Products.FindAsync(productId)
                ?? throw new KeyNotFoundException("Product no longer exists");

            if (userCart.Products.Contains(product))
                throw new ItemAlreadyInCartException("Item alredy in the cart");

            userCart.Products.Add(product);
            userCart.TotalPrice += product.Price;
            await _context.SaveChangesAsync();
            return userCart;
        }
        public async Task<CartItem> RemoveFromCartAsync(Guid customerID, Guid productId)
        {
            var userCart = await GetOrCreateCartItemAsync(customerID);
            var product = await _context.Products.FindAsync(productId)
                ?? throw new KeyNotFoundException("Product does nor exist or is deleted");
                
            if (!userCart.Products.Contains(product))
                throw new ItemNotFoundException("Product Already removed");

            userCart.Products.Remove(product);
            userCart.TotalPrice -= product.Price;

            await _context.SaveChangesAsync();

            return userCart;
        }

        private async Task<CartItem> GetOrCreateCartItemAsync(Guid customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId)
                ?? throw new KeyNotFoundException("Customer is disabled");

            var cart = await _context.CartItems.FindAsync(customer.CustomerID);

            if (cart is null)
            {
                var userCart = new CartItem
                {
                    CartId = customer.CustomerID,
                    CustomerId = customer.CustomerID,
                    Customer = customer,
                    Products = []
                };

                _context.CartItems.Add(userCart);
                await _context.SaveChangesAsync();

                return userCart;
            }
            return cart;
        }
    }


    public class CustomerNotFoundException(string message) : Exception(message);
    public class ItemAlreadyInCartException(string message) : Exception(message);
    public class ItemNotFoundException(string message) : Exception(message);
    public class ProductNotFoundException(string message) : Exception(message);
}
