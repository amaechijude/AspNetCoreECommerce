using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreEcommerce.Data;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreEcommerce.Repositories.Implementations
{
    public class PaymentRepository(ApplicationDbContext context) : IPaymentRepository
    {
        private readonly ApplicationDbContext _context = context;
        // private readonly IHttpClientFactory _httpClientFactory = new();

        public async Task<(string, string)> CheckoutAsync(Guid customerid, Guid orderId)
        {
            var customer = await _context.Customers.FindAsync(customerid)
                ?? throw new KeyNotFoundException("Customer not found");

            var order = await _context.Orders
                .Where(o => o.OrderId == orderId && o.Customer == customer)
                .FirstOrDefaultAsync()
                ?? throw new KeyNotFoundException("Order not found");

            if (order.PaymentStatus != PaymentStatusEnum.Pending)
                throw new PaymentStatusException("Duplicate Payment");

            return("success", "failure");
        }


        public class PaymentStatusException(string Message) : Exception(Message);
    }
}