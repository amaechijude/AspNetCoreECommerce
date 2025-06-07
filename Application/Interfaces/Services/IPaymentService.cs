using AspNetCoreEcommerce.Application.UseCases.PaymentUseCase;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<ResultPattern> InitiateTransaction(User user, PaymentDto dto);
        // Task<PaymentVerificationResponse?> VerifyTransaction(string paymentReference);
    }
}