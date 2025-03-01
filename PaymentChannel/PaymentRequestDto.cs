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
        public string? ResponseCode { get; set; }

        [JsonPropertyName("responseMessage")]
        public string? ResponseMessage { get; set; }

        [JsonPropertyName("responseBody")]
        public PaymentResponseBodyDto? ResponseBody { get; set; }
    }

    public class PaymentResponseBodyDto
    {
        [JsonPropertyName("paymentReference")]
        public string? PaymentReference { get; set; }

        [JsonPropertyName("transactionReference")]
        public string? TransactionReference { get; set; }

        [JsonPropertyName("checkoutUrl")]
        public string? CheckoutUrl { get; set; }
    }

 

public class PaymentVerificationResponse
    {
        [JsonPropertyName("requestSuccessful")]
        public bool RequestSuccessful { get; set; }

        [JsonPropertyName("responseCode")]
        public string? ResponseCode { get; set; }

        [JsonPropertyName("responseMessage")]
        public string? ResponseMessage { get; set; }

        [JsonPropertyName("responseBody")]
        public VerificationResponseBody? ResponseBody { get; set; }
    }

    public class VerificationResponseBody
    {
        [JsonPropertyName("domain")]
        public string? Domain { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("ercs_reference")]
        public string? ErcsReference { get; set; }

        [JsonPropertyName("tx_reference")]
        public string? TxReference { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("paid_at")]
        public DateTime PaidAt { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("channel")]
        public string? Channel { get; set; }

        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonPropertyName("metadata")]
        public string? Metadata { get; set; }

        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }

        [JsonPropertyName("fee_bearer")]
        public string? FeeBearer { get; set; }

        [JsonPropertyName("settled_amount")]
        public decimal SettledAmount { get; set; }

        [JsonPropertyName("customer")]
        public PayingCustomer? Customer { get; set; }
    }

    public class PayingCustomer
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("phone_number")]
        public string? PhoneNumber { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("reference")]
        public string? Reference { get; set; }
    }



}