using AspNetCoreEcommerce.Application.Interfaces.Repositories;
using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Domain.Enums;
using AspNetCoreEcommerce.Infrastructure.PaymentChannel;
using AspNetCoreEcommerce.Shared;
using Microsoft.Extensions.Options;

namespace AspNetCoreEcommerce.Application.UseCases.PaymentUseCase
{
    public class PaymentService(
        IPaymentRepository paymentRepository,
        ErcasPay ercasPay,
        PayStack payStack
        ) : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository = paymentRepository; 
        private readonly ErcasPay _ercasPay = ercasPay;
        private readonly PayStack _payStack = payStack;

        public async Task<ResultPattern> InitiateTransaction(User user, PaymentDto dto)
        {
            var order = await _paymentRepository.GetUserOrderById(user.Id, dto.OrderId);
            if (order is null)
                return ResultPattern.FailResult("Invalid Order");

            var initiateTransaction = PrepareInitiateTransactionDto(user, order);

            var payment = PreparePayment(user, order);
            await _paymentRepository.AddPayment(payment);
            var (success, error) = await _ercasPay.InitiateTransaction(initiateTransaction);

            if (success is not null)
                return ResultPattern.SuccessResult(success.responseBody.checkoutUrl);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return ResultPattern.FailResult(new
            {
                message = error.errorMessage,
                body = error.responseBody
            });
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }


        public async Task<ResultPattern> InitiatePayStack(User user, PaymentDto dto)
        {
            var order = await _paymentRepository.GetUserOrderById(user.Id, dto.OrderId);
            if (order is null)
                return ResultPattern.FailResult("Invalid Order");

            var initiateTransaction = PrepareInitiateTransactionDto(user, order);

            var payment = PreparePayment(user, order);
            await _paymentRepository.AddPayment(payment);
            var (success, error) = await _payStack.InitiateTransaction(initiateTransaction);

            if (success is not null)
                return ResultPattern.SuccessResult(success.responseBody.checkoutUrl);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return ResultPattern.FailResult(new
            {
                message = error.errorMessage,
                body = error.responseBody
            });
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        private static InitiateTransactionDto PrepareInitiateTransactionDto(User user, Order order)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            return new InitiateTransactionDto
            {
                amount = order.TotalAmountToBePaid,
                paymentReference = order.OrderRefrence,
                customerEmail = user.Email,
                customerName = "FirstName LastName",
                metadata = new Metadata
                {
                    firstname = user.FirstName,
                    lastname = user.LastName,
                    email = user.Email
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