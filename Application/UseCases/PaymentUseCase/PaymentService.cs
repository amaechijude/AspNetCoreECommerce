using AspNetCoreEcommerce.Application.Interfaces.Repositories;
using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Domain.Enums;
using AspNetCoreEcommerce.Infrastructure.PaymentChannel;
using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Application.UseCases.PaymentUseCase
{
    public class PaymentService(IPaymentRepository paymentRepository , ErcasPay ercasPay) : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository = paymentRepository; 
        private readonly ErcasPay _ercasPay = ercasPay;

        public async Task<object?> InitiateTransaction(Guid UserId, Guid OrderId)
        {
            var customer = await _paymentRepository.GetUserByIdAsync(UserId);
            if (customer is null)
                return ResultPattern.FailResult("Invalid User");
            var order = await _paymentRepository.GetUserOrderById(UserId, OrderId);
            if (order is null)
                return ResultPattern.FailResult("Invalid Order");

            var initiateTransaction = PrepareInitiateTransactionDto(customer, order);

            var payment = PreparePayment(customer, order);
            await _paymentRepository.AddPaymentAsync(payment);
            return await _ercasPay.InitiateTransaction(initiateTransaction);
        }



        private static InitiateTransactionDto PrepareInitiateTransactionDto(User customer, Order order)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            return new InitiateTransactionDto
            {
                amount = order.TotalAmountToBePaid,
                paymentReference = order.OrderRefrence,
                customerEmail = customer.Email,
                customerName = $"{customer.FirstName} {customer.LastName}",
                metadata = new Metadata
                {
                    firstname = customer.FirstName,
                    lastname = customer.LastName,
                    email = customer.Email
                }

            };
#pragma warning restore CS8601 // Possible null reference assignment.
        }

        private static Payment PreparePayment(User user, Order order)
        {
            return new Payment
            {
                PaymentId = Guid.CreateVersion7(),
                OrderId = order.OrderId,
                Order = order,
                PaymentReference = order.OrderRefrence,
                UserId = user.Id,
                User = user,
                TotalAmount = order.TotalAmountToBePaid,
                PaymentStatus = PaymentStatusEnum.Pending,
                CreatedDate = DateTimeOffset.UtcNow

            };
        }

    }
}