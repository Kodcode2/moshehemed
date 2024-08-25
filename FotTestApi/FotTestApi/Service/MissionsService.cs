using System.Diagnostics;
using System.Reflection;
using FotTestApi.Data;
using FotTestApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FotTestApi.Service
{

	public class MissionsService : IMissionsService
	{
		private readonly ApplicationBDContext _context;
		private static Dictionary<(int idAgent, int IdTarget), MissionModel> _DictOfmissions = new();
		private static Dictionary<int, Stopwatch> _stopwatch = new();
		private TimeSpan _elapsedTime;


		public MissionsService(ApplicationBDContext context)
		{
			_context = context;

		}
		public MissionModel CreateNewMission(AgentModel agent, TargetModel target, double distance)
		{

			return new()
			{
				Agent = agent,
				Target = target,
				IdAgent = agent.Id,
				IdTarget = target.Id,
				RemainingTime = TimeSpan.FromHours(distance / 5).ToString(@"hh\:mm\:ss"),
				Status = StatusMission.Proposal
			};
		}

		public async Task CheckIfCreateMission()
		{
			List<AgentModel> agents = await _context.Agents
				.Where(a => a.Status == StatusAgent.Inactive).ToListAsync();
			List<TargetModel> targets = await _context.Targets
				.Where(a => a.Status == StatusTarget.Live).ToListAsync();
			MissionModel? newMission = null;
			foreach (var agent in agents)
			{
				foreach (var target in targets)
				{
					double distance = CalculateDistance(agent.Location_x, agent.Location_y, target.Location_x, target.Location_y);
					if (distance < 200)
					{
						if (await CheckIfMissionExist(agent, target))
						{
							continue;
						}

						newMission = CreateNewMission(agent, target, distance);
						await _context.Missions.AddAsync(newMission);
						await _context.SaveChangesAsync();

					}
				}
			}

		}
		public async Task<bool> CheckIfMissionExist(AgentModel agent, TargetModel target)
		{
			_DictOfmissions = await _context.Missions
				.ToDictionaryAsync(k => (k.Agent.Id, k.Target.Id), v => v);
			return _DictOfmissions.ContainsKey((agent.Id, target.Id));

		}
		public static double CalculateDistance(double x1, double y1, double x2, double y2)
		{
			return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
		}

		public async Task<List<MissionModel>> GetAllMissions() =>
	await _context.Missions
		.Include(m => m.Agent)
		.Include(m => m.Target)
		.ToListAsync();

		public async Task<List<MissionModel>> GetMissionsByStatus(StatusMission status) =>
			await _context.Missions.Include(m => m.Agent)
		.Include(m => m.Target)
			.Where(m => m.Status == status).ToListAsync();


		public async Task StartTrackink(int idMission)
		{
			try
			{
				MissionModel? mission = await GetMissionById(idMission);
				if (mission == null)
				{
					return;
				}
				mission.Status = StatusMission.action;
				mission.Agent.Status = StatusAgent.Active;
				mission.Target.Status = StatusTarget.monitoredByAnAgent;
				_stopwatch[mission.Id] = new Stopwatch();
				_stopwatch[mission.Id].Start();
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception($"{ex.Message}", ex);

			}

		}

		public async Task GetMissionAtStatusActiveForSteps()
		{
			try
			{
				List<MissionModel> missionsAtStatusActive = await GetMissionsByStatus(StatusMission.action);

				List<Task> tasks = missionsAtStatusActive.Select(m => StepsTheAnAgentToTarget(m)).ToList();
				await Task.WhenAll(tasks);
			}
			catch (Exception ex)
			{
				throw new Exception($"{ex.Message}", ex);
			}
		}
		public async Task StepsTheAnAgentToTarget(MissionModel mission)
		{
			int agent_x = mission.Agent.Location_x;
			int agent_y = mission.Agent.Location_y;
			int target_x = mission.Target.Location_x;
			int target_y = mission.Target.Location_y;

			int xDirection = agent_x < target_x ? 1 : (agent_x > target_x ? -1 : 0);
			int yDirection = agent_y < target_y ? 1 : (agent_y > target_y ? -1 : 0);

			agent_x += xDirection;
			agent_y += yDirection;

			mission.Agent.Location_x = agent_x;
			mission.Agent.Location_y = agent_y;

			if (agent_x == target_x && agent_y == target_y)
			{
				await UpdateAtEndOfTheMission(mission.Id);
				return;
			}
			await _context.SaveChangesAsync();

		}
		public async Task<MissionModel?> GetMissionById(int id) =>
			await _context.Missions.Include(x => x.Agent).Include(x => x.Target)
			.FirstOrDefaultAsync(x => x.Id == id);


		public async Task UpdateAtEndOfTheMission(int id)
		{
			MissionModel? missionAccomplished = await GetMissionById(id);
			if (missionAccomplished == null)
			{
				return;
			}
			missionAccomplished.Status = StatusMission.Actioncompleted;
			missionAccomplished.Target.Status = StatusTarget.Eliminated;
			missionAccomplished.Agent.Status = StatusAgent.Inactive;
			_stopwatch[missionAccomplished.Id].Stop();
			missionAccomplished.ActualExecutionTime = _stopwatch[missionAccomplished.Id].Elapsed.ToString(@"hh\:mm\:ss");
			await _context.SaveChangesAsync();
		}
	}
}