using FotTestApi.Models;
namespace FotTestApi.Service
{
	public interface IMissionsService
	{
		MissionModel? CreateNewMission(AgentModel agent, TargetModel target, double distance);
		Task CheckIfCreateMission();
		Task<List<MissionModel>> GetAllMissions();
		Task<List<MissionModel>> GetMissionsByStatus(StatusMission status);
		Task StartTrackink(int id);
		Task GetMissionAtStatusActiveForSteps();
		Task StepsTheAnAgentToTarget(MissionModel mission);
		Task UpdateAtEndOfTheMission(int id);
	}
}
