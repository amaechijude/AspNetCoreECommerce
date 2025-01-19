using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Respositories.Contracts;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreEcommerce.Services.Implementations
{
    public class CustomerService(ICustomerRepository customerRepository)
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;
        private readonly PasswordHasher<Customer> _passwordhasher = new();

        public async Task<CustomerDTO?> CreateCustomerAsync(CustomerRegistrationDTO customerDto)
        {
            if (string.IsNullOrWhiteSpace(customerDto.CustomerEmail) || string.IsNullOrWhiteSpace(customerDto.Password))
                throw new ArgumentException("Email and Password cannot be empty");

            var newCustomer = new Customer
            {
                CustomerEmail = customerDto.CustomerEmail,
                CustomerName = customerDto.CustomerPhone,
                CustomerPhone = customerDto.CustomerPhone,
                DateJoined = DateTime.UtcNow
            };
            newCustomer.PasswordHash = _passwordhasher.HashPassword(newCustomer, customerDto.Password);
            var customer = await _customerRepository.CreateCustomerAsync(newCustomer);

            return new CustomerDTO
            {
                CustomerId = customer.CustomerID,
                CustomerEmail = customer.CustomerEmail,
                CustomerName = customer.CustomerName,
                CustomerPhone = customer.CustomerPhone,
            };
        }
        public async Task<CustomerDTO> GetCustomerByIdAsync(int id)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerByIdAsync(id);
                return new CustomerDTO
                {
                    CustomerId = customer.CustomerID,
                    CustomerEmail = customer.CustomerEmail,
                    CustomerName = customer.CustomerName,
                    CustomerPhone = customer.CustomerPhone,
                };
            }

            catch (KeyNotFoundException ex) { throw new KeyNotFoundException($"{ex}"); }
        }
        public async Task DeleteCustomerAsync(int customerId)
        {
            try { await _customerRepository.DeleteCustomerAsync(customerId); }

            catch (KeyNotFoundException ex) { throw new KeyNotFoundException($"{ex}"); }
        }
    }
}