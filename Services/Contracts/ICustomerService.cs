﻿using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.ResultResponse;

namespace AspNetCoreEcommerce.Services.Contracts
{
    public interface ICustomerService
    {
        Task<ResultPattern> CreateCustomerAsync(CustomerRegistrationDTO customer);
        Task<ResultPattern> GetCustomerByIdAsync(Guid id);
        Task<ResultPattern> DeleteCustomerAsync(Guid customerId);
        Task<ResultPattern> LoginCustomerAsync(LoginDto login);
        Task<ResultPattern> GetCustomerByEmailAsync(string email);
        Task<ResultPattern> UpdateCustomerAsync(Guid customerId, UpdateCustomerDto customer);
        Task<ResultPattern> VerifyCodeAsync(VerificationRequest verificationRequest);
    }
}
