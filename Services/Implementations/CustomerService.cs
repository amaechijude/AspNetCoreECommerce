using AspNetCoreEcommerce.Authentication;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Respositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetCoreEcommerce.Services.Implementations
{
    public class CustomerService(ICustomerRepository customerRepository, TokenProvider tokenProvider)
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;
        private readonly PasswordHasher<Customer> _passwordhasher = new();
        private readonly TokenProvider _tokenProvider = tokenProvider;

        public async Task<CustomerDTO?> CreateCustomerAsync(CustomerRegistrationDTO customerDto)
        {
            if (string.IsNullOrWhiteSpace(customerDto.CustomerEmail) || string.IsNullOrWhiteSpace(customerDto.Password))
                throw new ArgumentException("Email and Password cannot be empty");

            var newCustomer = new Customer
            {
                CustomerEmail = customerDto.CustomerEmail,
                CustomerName = customerDto.CustomerPhone,
                CustomerPhone = customerDto.CustomerPhone,
                DateJoined = DateTimeOffset.UtcNow
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
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            return new CustomerDTO
            {
                CustomerId = customer.CustomerID,
                CustomerEmail = customer.CustomerEmail,
                CustomerName = customer.CustomerName,
                CustomerPhone = customer.CustomerPhone,
            };
        }
        public async Task DeleteCustomerAsync(int customerId)
        {
             await _customerRepository.DeleteCustomerAsync(customerId);
        }

        public async Task<CustomerLoginViewDto> LoginCustomerAsync(LoginDto login)
        {
            var customer = await _customerRepository.GetCustomerByEmailAsync(login.Email);
#pragma warning disable CS8604 // Possible null reference argument.
            var verifyLogin = _passwordhasher.VerifyHashedPassword(customer, customer.PasswordHash, login.Password);
#pragma warning restore CS8604 // Possible null reference argument.

            if (verifyLogin == PasswordVerificationResult.Failed)
                throw new ArgumentException("Incorrect Password");
            
            var token = _tokenProvider.Create(customer);
            return new CustomerLoginViewDto 
            {
                CustomerId = customer.CustomerID,
                CustomerEmail = customer.CustomerEmail,
                Token = token
            };
        }
    }
}
