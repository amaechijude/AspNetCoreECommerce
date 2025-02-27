using System.Text.Json.Serialization;

namespace AspNetCoreEcommerce.PaymentChannel
{
    public class PaymentRequestDto
    {
        public required decimal Amount { get; set; }
        public required string PaymentReference { get; set; }
        public string PaymentMethods { get; set; } = "bank-transfer,ussd,card,qrcode";
        public required string CustomerName { get; set; }
        public required string CustomerEmail { get; set; }
        public required string CustomerPhoneNumber { get; set; }
        public string? RedirectUrl { get; set; }
        public string? Description { get; set; }
        public string Currency { get; set; } = "NGN";
        public string FeeBearer { get; set; } = "customer";
        public Metadata? Metadata { get; set; }
    }

    public class Metadata
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
    }


public class PaymentResponseDto
{
    [JsonPropertyName("requestSuccessful")]
    public bool RequestSuccessful { get; set; }

    [JsonPropertyName("responseCode")]
    public int ResponseCode { get; set; }

    [JsonPropertyName("responseMessage")]
    public string? ResponseMessage { get; set; }

    [JsonPropertyName("responseBody")]
    public ResponseBodyDto? ResponseBody { get; set; }
}

public class ResponseBodyDto
{
    [JsonPropertyName("paymentReference")]
    public string? PaymentReference { get; set; }

    [JsonPropertyName("transactionReference")]
    public string? TransactionReference { get; set; }

    [JsonPropertyName("checkoutUrl")]
    public string? CheckoutUrl { get; set; }
}


}