using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCoreEcommerce.Infrastructure.PaymentChannel
{
#pragma warning disable IDE1006 // Naming Styles
    public class InitiateTransactionDto
    {
        [Column(TypeName = "decimal(18,2)")]
        public required decimal amount { get; set; }
        public required string paymentReference { get; set; }
        public string paymentMethods { get; set; } = "bank-transfer,ussd,card,qrcode";
        public required string customerName { get; set; }
        public required string customerEmail { get; set; }
        public string? customerPhoneNumber { get; set; }
        public string? redirectUrl { get; set; }
        public string? description { get; set; }
        public string currency { get; set; } = "NGN";
        public string feeBearer { get; set; } = "customer";
        public Metadata? metadata { get; set; }
    }

    public class Metadata
    {
        public string? firstname { get; set; }
        public string? lastname { get; set; }
        public string? email { get; set; }
    }


    public class InitiateTransactionSuccessResponse
    {
        public bool requestSuccessful { get; set; }
        public string? responseCode { get; set; }
        public string? responseMessage { get; set; }
        public Responsebody responseBody { get; set; } = new();
    }

    public class Responsebody
    {
        public string paymentReference { get; set; } = string.Empty;
        public string transactionReference { get; set; } = string.Empty;
        public string checkoutUrl { get; set; } = string.Empty;
    }


    public class InitiateTransactionErrorResponse
    {
        public bool requestSuccessful { get; set; }
        public string responseCode { get; set; } = string.Empty;
        public string errorMessage { get; set; } = string.Empty;
        public List<object> responseBody { get; set; } = [];
    }


}