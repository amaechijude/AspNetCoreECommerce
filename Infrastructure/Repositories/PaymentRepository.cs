using AspNetCoreEcommerce.Application.Interfaces.Repositories;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreEcommerce.Infrastructure.Repositories
{
    public class PaymentRepository(ApplicationDbContext context) : IPaymentRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<User?> GetUserByIdAsync(Guid UserId)
        {
            var customer = await _context.Users.FindAsync(UserId);
                return customer is null
                ? null
                : customer;
        }

        public async Task<Order?> GetUserOrderById(Guid UserId, Guid OrderId)
        {
            var order = await _context.Orders
                .Where(o => o.OrderId == OrderId && o.UserId == UserId)
                .FirstOrDefaultAsync();
            return order is null
                ? null
                : order;
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

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}