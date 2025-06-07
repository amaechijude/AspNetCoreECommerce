using System.Text;
using System.Text.Json;
using AspNetCoreEcommerce.Application.UseCases.PaymentUseCase;
using Microsoft.Extensions.Options;

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

        public async Task<(InitiateTransactionSuccessResponse?, InitiateTransactionErrorResponse?)> InitiateTransaction(InitiateTransactionDto initiateTransaction)
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
                return (JsonSerializer.Deserialize<InitiateTransactionSuccessResponse>(responseBody), null);

            return (null, JsonSerializer.Deserialize<InitiateTransactionErrorResponse>(responseBody));
        }

    }

    public class PayStack(IOptions<PayStackOptions> payStackOptions)
    {
        private readonly IOptions<PayStackOptions> _paystackOptions = payStackOptions;
        private readonly string _baseURL = "https://api.paystack.co/transaction/initialize";
        private readonly HttpClient _httpClient = new();

        public async Task<(InitiateTransactionSuccessResponse?, InitiateTransactionErrorResponse?)> InitiateTransaction(InitiateTransactionDto initiateTransaction)
        {
            var url = $"{_baseURL}/payment/initiate";
            string jsonBody = JsonSerializer.Serialize(initiateTransaction);

            // Create request content
            StringContent content = new(jsonBody, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _paystackOptions.Value.PayStackSecretKey);

            HttpResponseMessage response = await client.PostAsync(url, content);

            string responseBody = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return (JsonSerializer.Deserialize<InitiateTransactionSuccessResponse>(responseBody), null);

            return (null, JsonSerializer.Deserialize<InitiateTransactionErrorResponse>(responseBody));
        }
    }
}
