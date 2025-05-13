using System.Text.Json.Serialization;

namespace AspNetCoreEcommerce.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatusEnum
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Canceled
    }
}