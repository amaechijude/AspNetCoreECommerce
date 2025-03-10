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
        public async Task<Customer> CreateCustomerAsync(Customer customer, string verificationCode)
        {
            await CreateVerificationCodeAsync(customer.CustomerEmail, verificationCode);
            var cart = new Cart
            {
                CartId = Guid.CreateVersion7(),
                CustomerId = customer.CustomerID,
                Customer = customer,
                CreatedAt = DateTimeOffset.UtcNow
            };
            customer.CartId = cart.CartId;
            customer.Cart = cart;
            _context.Customers.Add(customer);
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return customer;
        }
        public async Task<Customer?> GetCustomerByIdAsync(Guid id)
        {
            var cs = await _context.Customers.FindAsync(id);
            return cs is null ? null : cs;
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
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerEmail == email);
            return customer is null ? null : customer;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        private async Task CreateVerificationCodeAsync(string email, string code)
        {
            var verification = await _context.VerificationCodes.FindAsync(email);
            if (verification is not null)
                return;
            _ = new VerificationCode { Email = email, Code = code };
        }

    }
}
