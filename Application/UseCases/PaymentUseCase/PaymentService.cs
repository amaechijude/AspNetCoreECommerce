using AspNetCoreEcommerce.Application.Interfaces.Repositories;
using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Domain.Enums;
using AspNetCoreEcommerce.Infrastructure.PaymentChannel;
using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Services.Implementations
{
    public class PaymentService(IPaymentRepository paymentRepository , ErcasPay ercasPay) : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository = paymentRepository; 
        private readonly ErcasPay _ercasPay = ercasPay;

        public async Task<object?> InitiateTransaction(Guid CustomerId, Guid OrderId)
        {
            var customer = await _paymentRepository.GetCustomerByIdAsync(CustomerId);
            if (customer is null)
                return ResultPattern.FailResult("Invalid Customer");
            var order = await _paymentRepository.GetCustomerOrderById(CustomerId, OrderId);
            if (order is null)
                return ResultPattern.FailResult("Invalid Order");

            var initiateTransaction = PrepareInitiateTransactionDto(customer, order);

            var payment = PreparePayment(customer, order);
            await _paymentRepository.AddPaymentAsync(payment);
            return await _ercasPay.InitiateTransaction(initiateTransaction);
        }



        private static InitiateTransactionDto PrepareInitiateTransactionDto(Customer customer, Order order)
        {
            return new InitiateTransactionDto
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
        }

        private static Payment PreparePayment(Customer customer, Order order)
        {
            return new Payment
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
        }

    }
}