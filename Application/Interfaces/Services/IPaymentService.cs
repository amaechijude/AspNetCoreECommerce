namespace AspNetCoreEcommerce.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<object?> InitiateTransaction(Guid UserId, Guid OrderId);
        // Task<PaymentVerificationResponse?> VerifyTransaction(string paymentReference);
    }
}