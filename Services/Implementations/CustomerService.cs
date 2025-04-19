using System.Threading.Channels;
using AspNetCoreEcommerce.Authentication;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.EmailService;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Repositories.Contracts;
using AspNetCoreEcommerce.ResultResponse;
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

            var customerExists = await _customerRepository.GetCustomerByEmailAsync(customerDto.CustomerEmail);
            if (customerExists is not null)
                return ResultPattern.FailResult("Email already exists");
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
            var verificationCode = GlobalConstants.GenerateVerificationCode();
            newCustomer.VerificationCode = verificationCode;
            var customer = await _customerRepository.CreateCustomerAsync(newCustomer);
            var EmailData = new EmailDto
            {
                EmailTo = newCustomer.CustomerEmail,
                Name = $"{newCustomer.FirstName} {newCustomer.LastName}",
                Subject = "Welcome to our store",
                Body = "Welcome to our store, we are glad to have you as a customer \n" +
                    "Your verification code is: \n" +
                    $"{verificationCode}"
            };
            await _emailChannel.Writer.WriteAsync(EmailData);
            return ResultPattern.SuccessResult(MapCustomerDto(customer), "Customer created successfully");
        }

        public async Task<ResultPattern> VerifyCodeAsync(VerificationRequest verification)
        {
            if (string.IsNullOrEmpty(verification.Email))
                return ResultPattern.FailResult($"");
            if (string.IsNullOrWhiteSpace(verification.Code))
                return ResultPattern.FailResult("Invalid Code");
            var customer = await _customerRepository.GetCustomerByEmailAsync(verification.Email);
            if (customer is null)
                return ResultPattern.FailResult("Customer details not found");

            if (verification.Code != customer.VerificationCode)
                return ResultPattern.FailResult("Verification Failed. Try again later");

            customer.IsVerified = true;
            await _customerRepository.SaveChangesAsync();
            return ResultPattern.SuccessResult("Verified", "Customer is Verified");
        }
        
        public async Task<ResultPattern> UpdateCustomerAsync(Guid customerId, UpdateCustomerDto customerDto)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
            if (customer is null)
                return ResultPattern.FailResult("Customer not found", 404);
            customer.UpdateCustomer(customerDto);
            await _customerRepository.SaveChangesAsync();
            return ResultPattern.SuccessResult(MapCustomerDto(customer), "Customer updated successfully");
        }

        public async Task<ResultPattern> GetCustomerByIdAsync(Guid id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer is null)
                return ResultPattern.FailResult("Customer not found", 404);

            return ResultPattern.SuccessResult(MapCustomerDto(customer), "Customer found");
        }

        public async Task<ResultPattern> GetCustomerByEmailAsync(string email)
        {
            var customer = await _customerRepository.GetCustomerByEmailAsync(email);
            if (customer is null)
                return ResultPattern.FailResult("Customer not found", 404);
            return ResultPattern.SuccessResult(MapCustomerDto(customer), "Customer found");
        }
        public async Task<ResultPattern> DeleteCustomerAsync(Guid customerId)
        {
            var data = await _customerRepository.DeleteCustomerAsync(customerId);
            if (data is null)
                return ResultPattern.FailResult("Customer not found", 404);
            return ResultPattern.SuccessResult(data, "Customer deleted successfully");
        }

        public async Task<ResultPattern> LoginCustomerAsync(LoginDto login)
        {
            if (string.IsNullOrWhiteSpace(login.Email))  
                return ResultPattern.FailResult("Email cannot be empty");

            var customer = await _customerRepository.GetCustomerByEmailAsync(login.Email);
            if (customer is null || string.IsNullOrEmpty(customer.PasswordHash))
                return ResultPattern.FailResult("Invalid Credentials", 401);

            var verifyLogin = _passwordhasher.VerifyHashedPassword(customer, customer.PasswordHash, login.Password);
            if (verifyLogin == PasswordVerificationResult.Failed)
                return ResultPattern.FailResult("Invalid Credentials", 401);

            var token = _tokenProvider.Create(customer);

            customer.LastLogin = DateTimeOffset.UtcNow;
            await _customerRepository.SaveChangesAsync();

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
                CustomerPhone = customer.CustomerPhone
            };
        }
        
    }
}
