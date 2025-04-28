using AspNetCoreEcommerce.Application.Interfaces.Repositories;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreEcommerce.Infrastructure.Repositories
{
    public class CustomerRepository(ApplicationDbContext context) : ICustomerRepository
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            var cart = await UserCart(customer);
            customer.CartId = cart.CartId;
            customer.Cart = cart;
            _context.Customers.Add(customer);
            _context.Carts.Add(cart);
            return customer;
        }
        public async Task<Customer?> GetCustomerByIdAsync(Guid id)
        {
            return await _context.Customers.FindAsync(id);
        }
        public async Task<string?> DeleteCustomerAsync(Guid id)
        {
            var cs = await _context.Customers.FindAsync(id);
            if (cs is null)
                return null;
            cs.IsDeleted = true;
            await _context.SaveChangesAsync();
            return "Customer deleted successfully";
        }

        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.User.Email == email);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        private async Task<Cart> UserCart(Customer customer)
        {
            var cart = await _context.Carts
                .FirstOrDefaultAsync (c => c.CustomerId == customer.CustomerID);
            if (cart is null)
            {
                return new Cart
                {
                    CartId = Guid.CreateVersion7(),
                    CustomerId = customer.CustomerID,
                    Customer = customer,
                    CreatedAt = DateTimeOffset.UtcNow
                };
            }
            return cart;
        }

        public async Task<Customer?> GetCustomerByUserIdAsync(Guid id)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.UserId == id);
        }
    }
}
