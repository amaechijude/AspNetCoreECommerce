using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreEcommerce.PaymentChannel;

namespace AspNetCoreEcommerce.Services.Contracts
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto?> InitiateTransaction(Guid CustomerId, Guid OrderId);
        Task<PaymentVerificationResponse?> VerifyTransaction(string paymentReference);
    }
}