using System.Threading.Channels;
using AspNetCoreEcommerce.Authentication;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.EmailService;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Respositories.Contracts;
using AspNetCoreEcommerce.Result;
using AspNetCoreEcommerce.Services.Contracts;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreEcommerce.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly TokenProvider _tokenProvider;
        private readonly Channel<EmailDto> _emailChannel;

        public CustomerService(
            ICustomerRepository customerRepository,
            TokenProvider tokenProvider,
            Channel<EmailDto> emailChannel
            )

        {
            _customerRepository = customerRepository;
            _tokenProvider = tokenProvider;
            _emailChannel = emailChannel;
        }

        private readonly PasswordHasher<Customer> _passwordhasher = new();

        public async Task<ResultPattern> CreateCustomerAsync(CustomerRegistrationDTO customerDto)
        {
            if (string.IsNullOrWhiteSpace(customerDto.CustomerEmail) || string.IsNullOrWhiteSpace(customerDto.Password))
                return ResultPattern.FailResult("Email and password cannot be empty");

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

            var data =  MapCustomerDto(customer);
            return ResultPattern.SuccessResult(data, "Customer created successfully");
        }

        public async Task<ResultPattern> GetCustomerByIdAsync(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer is null)
                return ResultPattern.FailResult("Customer not found", 404);
            var data = MapCustomerDto(customer);

            return ResultPattern.SuccessResult(data, "Customer found");
        }
        public async Task DeleteCustomerAsync(int customerId)
        {
            await _customerRepository.DeleteCustomerAsync(customerId);
        }

        public async Task<ResultPattern> LoginCustomerAsync(LoginDto login)
        {
            if (string.IsNullOrWhiteSpace(login.Email))  
                return ResultPattern.FailResult("Email cannot be empty");

            var customer = await _customerRepository.GetCustomerByEmailAsync(login.Email);
            if (customer is null)
                return ResultPattern.FailResult("Customer not found", 404);

#pragma warning disable CS8604 // Possible null reference argument.
            var verifyLogin = _passwordhasher.VerifyHashedPassword(customer, customer.PasswordHash, login.Password);
#pragma warning restore CS8604 // Possible null reference argument.

            if (verifyLogin == PasswordVerificationResult.Failed)
                return ResultPattern.FailResult("Invalid email or password");

            var token = _tokenProvider.Create(customer);

            customer.LastLogin = DateTimeOffset.UtcNow;
            await _customerRepository.SaveLastLoginDate();

            var data = new CustomerLoginViewDto
            {
                CustomerId = customer.CustomerID,
                CustomerEmail = customer.CustomerEmail,
                LastLoginDate = customer.LastLogin,
                Token = token
            };
            return ResultPattern.SuccessResult(data, "Login successful");
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
