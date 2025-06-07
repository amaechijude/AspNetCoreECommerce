using System.ComponentModel.DataAnnotations;

namespace AspNetCoreEcommerce.Application.UseCases.PaymentUseCase
{
    public class PayStackOptions
    {
        [Required, MinLength(10)]
        public string PayStackSecretKey { get; set; } = string.Empty;
    }
}
