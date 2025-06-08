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

            var payment = PreparePayment(user, order);
            await _paymentRepository.AddPayment(payment);

            var initiateTransaction = PrepareErcasPayInitiateTransaction(user, order);
            var (success, error) = await _ercasPay.InitiateTransaction(initiateTransaction);

            if (success is not null)
                return ResultPattern.SuccessResult(success.responseBody.checkoutUrl);

            return ResultPattern.FailResult(new
            {
                message = error?.errorMessage,
                body = error?.responseBody
            });
        }


        public async Task<ResultPattern> InitiatePayStack(User user, PaymentDto dto)
        {
            var order = await _paymentRepository.GetUserOrderById(user.Id, dto.OrderId);
            if (order is null)
                return ResultPattern.FailResult("Invalid Order");

            var payment = PreparePayment(user, order);
            await _paymentRepository.AddPayment(payment);

            var init = new PayStackRequestBody
            {
                email = $"{user.Email}",
                amount = $"{Math.Round((order.TotalAmountToBePaid * 100), 2)}", // Paystack amount is in kobo
                currency = "NGN"
            };

            var (success, error) = await _payStack.InitiatePaystackTransaction(init);
            if (success is not null && error is null)
                return ResultPattern.SuccessResult(success.data.authorization_url);
            
            if (error is not null)
            return ResultPattern.FailResult(error.message);

            return ResultPattern.FailResult("Initialisation Failed. Try Paying with ErcasPay");
        }

        private static InitiateErcasPayTransactionDto PrepareErcasPayInitiateTransaction(User user, Order order)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            return new InitiateErcasPayTransactionDto
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