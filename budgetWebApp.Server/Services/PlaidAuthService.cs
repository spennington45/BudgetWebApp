using budgetWebApp.Server.Helpers;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace budgetWebApp.Server.Services
{
    public class PlaidAuthService
    {
        private readonly HttpClient _httpClient;

        public PlaidAuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAccessTokenAsync(string publicToken)
        {
            var requestBody = new
            {
                client_id = PlaidConfig.ClientId,
                secret = PlaidConfig.SandboxSecret,
                public_token = publicToken
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://sandbox.plaid.com/item/public_token/exchange", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new ApplicationException($"Plaid token exchange failed: {error}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseContent);
            var accessToken = doc.RootElement.GetProperty("access_token").GetString();

            return accessToken ?? throw new ApplicationException("Access token not found in Plaid response.");
        }

        public async Task<string> CreateLinkTokenAsync(string userId)
        {
            var requestBody = new
            {
                client_id = PlaidConfig.ClientId,
                secret = PlaidConfig.SandboxSecret,
                client_name = "My Budget App",
                language = "en",
                country_codes = new[] { "US" },
                user = new { client_user_id = userId },
                products = new[] { "auth", "transactions" }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://sandbox.plaid.com/link/token/create", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new ApplicationException($"Plaid link token creation failed: {responseContent}");

            using var doc = JsonDocument.Parse(responseContent);
            return doc.RootElement.GetProperty("link_token").GetString();
        }
    }
}