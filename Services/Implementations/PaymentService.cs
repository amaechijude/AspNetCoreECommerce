using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<PaymentResponseDto?> InitiateTransaction(Guid CustomerId, Guid OrderId)
        {
            var (customer, order) = await _paymentRepository.GetCustomerAndIdAsync(CustomerId, OrderId);
            var paymentRequest = new PaymentRequestDto
            {
                Amount = order.TotalAmountToBePaid,
                PaymentReference = order.OrderRefrence,
                CustomerEmail = customer.CustomerEmail,
                CustomerName = $"{customer.FirstName} {customer.LastName}",
                CustomerPhoneNumber = customer.CustomerPhone ?? "+234000000000",
                Metadata = new Metadata
                {
                    Firstname = customer.FirstName,
                    Lastname = customer.LastName,
                    Email = customer.CustomerEmail
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
            return await _ercasPay.InitiateTransaction(paymentRequest);
        }

        public async Task<PaymentVerificationResponse?> VerifyTransaction(string paymentReference)
        {
            return await _ercasPay.VerifyTransaction(paymentReference);
        }
    }
}