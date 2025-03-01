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

        public async Task<(Customer, Order)> GetCustomerAndIdAsync(Guid CustomerId, Guid OrderId)
        {
            var customer = await _context.Customers.FindAsync(CustomerId)
                ?? throw new KeyNotFoundException("Customer not found");

            var order = await _context.Orders
                .Where(o => o.OrderId == OrderId && o.CustomerId == CustomerId)
                .FirstOrDefaultAsync()
                ?? throw new KeyNotFoundException("Order not found");

            return (customer, order);
        }

        public async Task AddPaymentAsync(Payment payment)
        {
            var existingPayment = await _context.Payments
                .FirstOrDefaultAsync(p => p.OrderId == payment.OrderId);

            if (existingPayment != null)
                return; // Avoid duplicate insertion

            try
            {
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return; // Log the exception
            }
        }
    }
}