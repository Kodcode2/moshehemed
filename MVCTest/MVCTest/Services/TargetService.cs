using MVCTest.Models;
using System.Text.Json;

namespace MVCTest.Services
{
    public class TargetService(IHttpClientFactory httpClientFactory) : ITargetService
    {
        private readonly string _baseUrl = "https://localhost:7051/";

        public async Task<List<TargetModel>?> GetAllAgentsAsync()
        {
            var httpClient = httpClientFactory.CreateClient();
            var result = await httpClient.GetAsync($"{_baseUrl}Targets");

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                List<TargetModel>? agents = JsonSerializer.Deserialize<List<TargetModel>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return agents;
            }
            return null;
        }
    }
}