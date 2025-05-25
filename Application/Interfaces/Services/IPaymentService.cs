using AspNetCoreEcommerce.Application.UseCases.PaymentUseCase;
using AspNetCoreEcommerce.Domain.Entities;

namespace AspNetCoreEcommerce.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<object?> InitiateTransaction(User user, PaymentDto dto);
        // Task<PaymentVerificationResponse?> VerifyTransaction(string paymentReference);
    }
}