namespace AspNetCoreEcommerce.Services.Contracts
{
    public interface IPaymentService
    {
        Task<object?> InitiateTransaction(Guid CustomerId, Guid OrderId);
        // Task<PaymentVerificationResponse?> VerifyTransaction(string paymentReference);
    }
}