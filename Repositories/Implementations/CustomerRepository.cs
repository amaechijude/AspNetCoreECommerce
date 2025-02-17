using AspNetCoreEcommerce.Data;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Respositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreEcommerce.Repositories.Implementations
{
    public class CustomerRepository(ApplicationDbContext context) : ICustomerRepository
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            var customerExists = await _context.Customers.AnyAsync(c => c.CustomerEmail == customer.CustomerEmail);
            if (customerExists)
                throw new DuplicateEmailException("Email already exists");
            _context.Customers.Add(customer);
             await _context.SaveChangesAsync();
              return customer;
        }
        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            var cs = await _context.Customers.FindAsync(id) ?? throw new KeyNotFoundException("Customer is does not exist or is deleted");
            return cs;
        }
        public async Task DeleteCustomerAsync(int id)
        {
            var cs = await _context.Customers.FindAsync(id) ?? throw new KeyNotFoundException("Customer is does not exist or is deleted");
            cs.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<Customer> GetCustomerByEmailAsync(string email)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerEmail == email)
               ?? throw new ArgumentException("Invalid Email");
            return customer;
        }
        public async Task SaveLastLoginDate()
        {
            await _context.SaveChangesAsync();
        }

        public class DuplicateEmailException(string message) : Exception(message);
    }
}
