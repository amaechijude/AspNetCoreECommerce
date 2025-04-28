using System.Text;
using System.Text.Json;

namespace AspNetCoreEcommerce.Infrastructure.PaymentChannel
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

        public async Task<object?> InitiateTransaction(InitiateTransactionDto initiateTransaction)
        {
            var url = $"{_baseURL}/payment/initiate";
            string jsonBody = JsonSerializer.Serialize(initiateTransaction);

            // Create request content
            StringContent content = new(jsonBody, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _ercasPayApiKey);

            HttpResponseMessage response = await client.PostAsync(url, content);

            string responseBody = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<InitiateTransactionSuccessResponse>(responseBody);

            return JsonSerializer.Deserialize<InitiateTransactionErrorResponse>(responseBody);
        }

    }
}
