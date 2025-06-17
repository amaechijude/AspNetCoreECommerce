using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.Application.UseCases.PaymentUseCase
{
    public class PaymentDto
    {
        [Required]
        public Guid OrderId { get; set; }
        [Required]
        public Guid ShippingAddrId { get; set; }
    }
}