using AspNetCoreEcommerce.Application.Interfaces.Repositories;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreEcommerce.Infrastructure.Repositories
{
    public class PaymentRepository(ApplicationDbContext context) : IPaymentRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<ShippingAddress?> GetShippingAddressByIdAsync(Guid UserId, Guid ShippingAddressId)
        {
            return await _context.ShippingAddresses
                .Where(s => s.UserId == UserId && s.ShippingAddressId == ShippingAddressId)
                .FirstOrDefaultAsync();
        }

        public async Task<Order?> GetUserOrderById(Guid UserId, Guid OrderId)
        {
            return await _context.Orders
                .Where(o => o.OrderId == OrderId && o.UserId == UserId)
                .FirstOrDefaultAsync();
        }


        public async Task<bool> CheckExistingPaymentAsync(Guid orderId)
        {
            return await _context.Payments.AnyAsync(p => p.OrderId == orderId);
        }
        public async Task<bool> AddPayment(Payment payment)
        {
            Payment? currentPayment = _context.Payments.FirstOrDefault(p => p.OrderId == payment.OrderId);
            if (currentPayment is not null) return false;
                
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}