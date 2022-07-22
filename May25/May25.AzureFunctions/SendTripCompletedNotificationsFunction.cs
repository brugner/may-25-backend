using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace May25.AzureFunctions
{
    public static class SendTripCompletedNotificationsFunction
    {
        [FunctionName("SendTripCompletedNotificationsFunction")]
        public static async Task Run([TimerTrigger("0 0 9 * * *")] TimerInfo timer, ILogger logger)
        {
            try
            {
                var token = await TokenService.GetJwtTokenAsync(logger);

                await SendNotificationsAsync(logger, token);

                logger.LogInformation($"Next occurrence: {timer.FormatNextOccurrences(1)}");
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }

        private static async Task SendNotificationsAsync(ILogger logger, string token)
        {
            var url = Environment.GetEnvironmentVariable("SendTripCompletedNotificationsEndpoint");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync(url, null);

            logger.LogInformation($"Success {response.IsSuccessStatusCode}");
            logger.LogInformation($"Status Code: {response.StatusCode}");

            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<APIResult>(responseBody);

            logger.LogInformation($"{result.Message}");
        }
    }
}
