using AspNetCoreEcommerce.Data;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Respositories.Contracts;

namespace AspNetCoreEcommerce.Respositories.Implementations
{
    public class CartItemRepository(ApplicationDbContext context) : ICartItemRepository
    {
        private readonly ApplicationDbContext _context = context;

        private async Task<CartItem> GetOrCreateCartItemAsync(Guid customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId)
                ?? throw new CustomerNotFoundException();

            var cart = _context.CartItems.FirstOrDefaultAsync(c => c.Customer == customer);
            if (cart is null)
            {
                var newCart = new CartItem
                {
                    CartId = Guid.CreateVersion7(),
            
                    
                }
            }

            var cart = new CartItem { Customer = customer };
            _context.CartItems.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }
        public async Task<CartItem> ADddToCartAsync(int customerID, int productId)
        {
            var userCart = await GetOrCreateCartItemAsync(customerID);
            var product = await _context.Products.FindAsync(productId)
                ?? throw new ProductNotFoundException();

            if (userCart.Products.Contains(product))
                throw new ItemAlreadyInCartException();

            userCart.Products.Add(product);
            await _context.SaveChangesAsync();
            return userCart;
        }
        public async Task RemoveFromCartAsync(int customerID, int productId)
        {
            var userCart = await GetOrCreateCartItemAsync(customerID);
            var product = await _context.Products.FindAsync(productId)
                ?? throw new ProductNotFoundException();
                
            if (!userCart.Products.Contains(product))
                throw new ItemNotFoundException();

            userCart.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }


    public class CustomerNotFoundException() : Exception("Customer does not exist or is deleted.");
    public class ItemAlreadyInCartException() : Exception("The item is already in the cart.");
    public class ItemNotFoundException() : Exception("The item is not in the cart.");
    public class ProductNotFoundException() : Exception("Product not found");
}
