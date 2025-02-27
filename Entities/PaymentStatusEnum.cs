using System.Text.Json.Serialization;

namespace AspNetCoreEcommerce.Entities
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PaymentStatusEnum
    {
        Pending = 1,
        verified = 2,
        Refunded = 4
    }
}