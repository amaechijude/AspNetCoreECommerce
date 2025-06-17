using System.Net.Http.Headers;
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

        public async Task<(InitiateTransactionSuccessResponse?, InitiateTransactionErrorResponse?)> InitiateTransaction(InitiateErcasPayTransactionDto initiateTransaction)
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
        public async Task<(PaystackSuccessResponse?, PaystackErrorResponse?)> InitiatePaystackTransaction(PayStackRequestBody dto)
        {
            var key = _paystackOptions.Value;
            var url = "https://api.paystack.co/transaction/initialize";
            string jsonBody = JsonSerializer.Serialize(dto);

            // Create request content
            StringContent content = new(jsonBody, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key.PayStackSecretKey);

            HttpResponseMessage response = await client.PostAsync(url, content);

            string responseBody = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var success = JsonSerializer.Deserialize<PaystackSuccessResponse>(responseBody);
                return (success, null);
            }
            var error = JsonSerializer.Deserialize<PaystackErrorResponse>(responseBody);
            return (null, error);
        }
    }


}
