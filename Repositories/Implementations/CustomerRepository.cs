using AspNetCoreEcommerce.Data;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using static AspNetCoreEcommerce.Repositories.Implementations.VendorRepository;

namespace AspNetCoreEcommerce.Repositories.Implementations
{
    public class CustomerRepository(ApplicationDbContext context) : ICustomerRepository
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            var customerExists = await _context.Customers.AnyAsync(c => c.CustomerEmail == customer.CustomerEmail);
            if (customerExists)
                throw new DuplicateException("Email already exists");

            var cart = new Cart { CartId = Guid.CreateVersion7(), CustomerId = customer.CustomerID, Customer = customer, CreatedAt = DateTimeOffset.UtcNow };
            customer.CartId = cart.CartId;
            customer.Cart = cart;
            _context.Customers.Add(customer);
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return customer;
        }
        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            var cs = await _context.Customers.FindAsync(id);
            return cs is null ? null : cs;
        }
        public async Task DeleteCustomerAsync(int id)
        {
            var cs = await _context.Customers.FindAsync(id)
                ?? throw new KeyNotFoundException("Customer is does not exist or is deleted");
            cs.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerEmail == email);
            return customer is null ? null : customer;
        }
        public async Task SaveLastLoginDate()
        {
            await _context.SaveChangesAsync();
        }

    }
}
