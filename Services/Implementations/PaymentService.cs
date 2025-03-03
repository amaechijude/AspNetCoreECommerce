using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.PaymentChannel;
using AspNetCoreEcommerce.Repositories.Contracts;
using AspNetCoreEcommerce.Services.Contracts;

namespace AspNetCoreEcommerce.Services.Implementations
{
    public class PaymentService(IPaymentRepository paymentRepository , ErcasPay ercasPay) : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository = paymentRepository; 
        private readonly ErcasPay _ercasPay = ercasPay;

        public async Task<object?> InitiateTransaction(Guid CustomerId, Guid OrderId)
        {
            var (customer, order) = await _paymentRepository.GetCustomerAndIdAsync(CustomerId, OrderId);
            var initiateTransaction = new InitiateTransactionDto
            {
                amount = order.TotalAmountToBePaid,
                paymentReference = order.OrderRefrence,
                customerEmail = customer.CustomerEmail,
                customerName = $"{customer.FirstName} {customer.LastName}",
                metadata = new Metadata
                {
                    firstname = customer.FirstName,
                    lastname = customer.LastName,
                    email = customer.CustomerEmail
                }
                
            };
            var payment = new Payment
            {
                PaymentId = Guid.CreateVersion7(),
                OrderId = order.OrderId,
                Order = order,
                PaymentReference = order.OrderRefrence,
                CustormerId = customer.CustomerID,
                Customer = customer,
                TotalAmount = order.TotalAmountToBePaid,
                PaymentStatus = PaymentStatusEnum.Pending,
                CreatedDate = DateTimeOffset.UtcNow

            };
            await _paymentRepository.AddPaymentAsync(payment);
            return await _ercasPay.InitiateTransaction(initiateTransaction);
        }

    }
}