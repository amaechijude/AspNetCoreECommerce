using System.Text;
using System.Text.Json;

namespace AspNetCoreEcommerce.PaymentChannel
{
    public class ErcasPay
    {
        private readonly string _ercasPayApiKey;

        public ErcasPay()
        {
            DotNetEnv.Env.TraversePath().Load();
            _ercasPayApiKey = $"{Environment.GetEnvironmentVariable("ERCASPAY_SECRET_KEY")}";
        }

        private readonly string _baseURL = "https://api.ercaspay.com/api/v1";
        private readonly HttpClient _httpClient = new();

        public async Task<PaymentResponseDto?> InitiateTransaction(PaymentRequestDto paymentRequest)
        {
            var url = $"{_baseURL}/payment/initiate";
            string jsonBody = JsonSerializer.Serialize(paymentRequest);
            // Create request content
            StringContent content = new(jsonBody, Encoding.UTF8, "application/json");
            HttpRequestMessage request = new(HttpMethod.Post, url);

            request.Headers.Add("Authorization", $"Bearer {_ercasPayApiKey}");
            request.Content = content;

            //send post request
            HttpResponseMessage response = await _httpClient.SendAsync(request);

            string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialise 
            var result = JsonSerializer.Deserialize<PaymentResponseDto>(responseBody);
            return result;

        }

        public async Task<PaymentVerificationResponse?> VerifyTransaction(string paymentReference)
        {
            var url = $"{_baseURL}/payment/verify/{paymentReference}";
            HttpRequestMessage request = new(HttpMethod.Get, url);
            request.Headers.Add("Authorization", $"Bearer {_ercasPayApiKey}");
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            string responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PaymentVerificationResponse>(responseBody);
            return result;
        }
    }
}
