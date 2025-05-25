using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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