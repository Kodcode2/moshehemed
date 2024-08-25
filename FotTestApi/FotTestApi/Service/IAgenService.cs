using FotTestApi.ModelDto;
using FotTestApi.Models;

namespace FotTestApi.Service
{
	public interface IAgenService
	{
		Task<AgentModel> CreateAsyncService(AgentDto agentDto);
		Task StartingPositionService(LocationDto location, int id);
		Task<AgentModel?> FindByIdService(int id);
		Task<AgentModel?> StepsAgent(string diraction, int id);
		Task<List<AgentModel>?> GetAllAgent();
	}
}
