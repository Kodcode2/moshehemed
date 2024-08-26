using System.Net.Http;
using System.Text.Json;
using MVCTest.Models;
using static MVCTest.Models.MissionModel;

namespace MVCTest.Services
{
    public class AgentService(IHttpClientFactory httpClientFactory) : IAgentService
    {
        private readonly string _baseUrl = "https://localhost:7051/";

        public async Task<List<AgentModel>?> GetAllAgentsAsync()
        {
            var httpClient = httpClientFactory.CreateClient();
            var result = await httpClient.GetAsync($"{_baseUrl}agents");

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                List<AgentModel>? agents = JsonSerializer.Deserialize<List<AgentModel>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return agents;
            }
            return null;
        }

        public async Task<MissionModel?> GetMissionByIdAgent(int idAgent)
        {
            var httpClient = httpClientFactory.CreateClient();
            var result = await httpClient.GetAsync($"{_baseUrl}Missions/missionsByStatus/{StatusMission.action}");

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                List<MissionModel>? missions = JsonSerializer.Deserialize<List<MissionModel>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return missions!.FirstOrDefault(x => x.IdAgent == idAgent);
            }
            return null;
        }
    }
}