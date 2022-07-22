using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace May25.AzureFunctions
{
    static class TokenService
    {
        internal static async Task<string> GetJwtTokenAsync(ILogger logger)
        {
            try
            {
                var url = Environment.GetEnvironmentVariable("AuthenticateEndpoint");
                var apiKey = Environment.GetEnvironmentVariable("APIKey");
                var client = new HttpClient();
                var json = JsonSerializer.Serialize(new { APIKey = apiKey });
                var requestData = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, requestData);
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<AuthenticationResult>(responseContent);

                return result.AccessToken;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Authentication failed ");
                throw;
            }
        }
    }
}
