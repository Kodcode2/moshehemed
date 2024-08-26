using System.Net.Http;
using System.Text.Json;
using MVCTest.Models;

namespace MVCTest.Services
{
	public class MissionService(IHttpClientFactory httpClientFactory) : IMissionService
	{
		private readonly string _baseUrl = "https://localhost:7051/";
		public async Task<List<MissionModel>?> GetAllMissionsAsync()
		{
			var httpClient = httpClientFactory.CreateClient();
			var result = await httpClient.GetAsync($"{_baseUrl}Missions");
			if (result.IsSuccessStatusCode)
			{
				var content = await result.Content.ReadAsStringAsync();
				List<MissionModel>? missions = JsonSerializer.Deserialize<List<MissionModel>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
				return missions;
			}
			return null;
		}
	}
}
