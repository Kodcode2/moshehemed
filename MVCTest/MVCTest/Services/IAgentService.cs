using MVCTest.Models;

namespace MVCTest.Services
{
    public interface IAgentService
    {
        Task<List<AgentModel>?> GetAllAgentsAsync();
        Task<MissionModel?> GetMissionByIdAgent(int idAgent);

    }
}
