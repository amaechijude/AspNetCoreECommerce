using System.Threading.Channels;
using AspNetCoreEcommerce.Authentication;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.EmailService;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Respositories.Contracts;
using AspNetCoreEcommerce.Services.Contracts;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreEcommerce.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly TokenProvider _tokenProvider;
        private readonly Channel<EmailDto> _emailChannel;

        public CustomerService(ICustomerRepository customerRepository, TokenProvider tokenProvider, Channel<EmailDto> emailChannel)
        {
            _customerRepository = customerRepository;
            _tokenProvider = tokenProvider;
            _emailChannel = emailChannel;
        }
        
        private readonly PasswordHasher<Customer> _passwordhasher = new();

        public async Task<CustomerDTO> CreateCustomerAsync(CustomerRegistrationDTO customerDto)
        {
            if (string.IsNullOrWhiteSpace(customerDto.CustomerEmail) || string.IsNullOrWhiteSpace(customerDto.Password))
                throw new ArgumentException("Email and Password cannot be empty");

            var newCustomer = new Customer
            {
                CustomerID = Guid.CreateVersion7(),
                CustomerEmail = customerDto.CustomerEmail,
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                CustomerPhone = customerDto.CustomerPhone,
                SignupDate = DateTimeOffset.UtcNow
            };
            newCustomer.PasswordHash = _passwordhasher.HashPassword(newCustomer, customerDto.Password);
            var customer = await _customerRepository.CreateCustomerAsync(newCustomer);

            return MapCustomerDto(customer);
        }

        public async Task<CustomerDTO> GetCustomerByIdAsync(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            return MapCustomerDto(customer);
        }
        public async Task DeleteCustomerAsync(int customerId)
        {
             await _customerRepository.DeleteCustomerAsync(customerId);
        }

        public async Task<CustomerLoginViewDto> LoginCustomerAsync(LoginDto login)
        {
            if (string.IsNullOrWhiteSpace(login.Email))
                throw new ArgumentException("Email cannot be empty");

            var customer = await _customerRepository.GetCustomerByEmailAsync(login.Email);

#pragma warning disable CS8604 // Possible null reference argument.
            var verifyLogin = _passwordhasher.VerifyHashedPassword(customer, customer.PasswordHash, login.Password);
#pragma warning restore CS8604 // Possible null reference argument.

            if (verifyLogin == PasswordVerificationResult.Failed)
                throw new ArgumentException("Incorrect Password");
            
            var token = _tokenProvider.Create(customer);
            
            customer.LastLogin = DateTimeOffset.UtcNow;
            await _customerRepository.SaveLastLoginDate();

            return new CustomerLoginViewDto 
            {
                CustomerId = customer.CustomerID,
                CustomerEmail = customer.CustomerEmail,
                LastLoginDate = customer.LastLogin,
                Token = token
            };
        }

        private static CustomerDTO MapCustomerDto(Customer customer)
        {
            return new CustomerDTO
            {
                CustomerId = customer.CustomerID,
                CustomerEmail = customer.CustomerEmail,
                CustomerName = $"{customer.FirstName} {customer.LastName}",
                CustomerPhone = customer.CustomerPhone,
            };
        }
    }
}
