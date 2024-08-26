using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Numerics;
using FotTestApi.Data;
using FotTestApi.Models;
using Microsoft.EntityFrameworkCore;


namespace FotTestApi.Service
{

    public class MissionService : IMissionsService
    {
        private readonly IServiceProvider _serviceProvider;
        private static Dictionary<(int idAgent, int IdTarget), MissionModel> _DictOfmissions = new();
        private static Dictionary<(int idAgent, int IdTarget), double> _DictOfDistance = new();
        private static Dictionary<int, Stopwatch> _stopwatch = new();
        private static List<(int idAgent, int IdTarget)> _allMissionsIntoDB = new();
        public MissionService(IServiceProvider serviceProvider, ApplicationBDContext context)
        {
            _serviceProvider = serviceProvider;
        }



        public MissionModel? CreateNewMission(AgentModel agent, TargetModel target, double distance)
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
            using var dbContext = DbContextFactory.CreateDbContext(_serviceProvider);

            List<AgentModel> agents = await dbContext.Agents
                .Where(a => a.Status == StatusAgent.Inactive).ToListAsync();

            List<TargetModel> livingTargets = await dbContext.Targets
                .Where(a => a.Status == StatusTarget.Live).ToListAsync();

            List<MissionModel> missions = await dbContext.Missions
                .Include(m => m.Agent).Include(m => m.Target)
                .ToListAsync();
            var agentTargetPairs = agents
           .SelectMany(agent => livingTargets, (agent, target) => new
           {
               Agent = agent,
               Target = target,
               Distance = CalculateDistanceDouble(agent, target)
           })
           .Where(pair =>
           pair.Distance <= 200.0
           && IsValidLocationAgen(pair.Agent)
           && IsValidLocationTarget(pair.Target))
           .ToList();

            List<MissionModel?> newMissions = agentTargetPairs
            .Where(pair => !missions.Any(m => m.Agent.Id == pair.Agent.Id && m.Target.Id == pair.Target.Id))
            .Select(pair => CreateNewMission(pair.Agent, pair.Target, pair.Distance)).ToList();
            if (newMissions.Any())
            {
                await dbContext.Missions.AddRangeAsync(newMissions!);
            }
            await dbContext.SaveChangesAsync();
        }
        public async Task<bool> CheckIfMissionExist(AgentModel agent, TargetModel target)
        {
            using var dbContext = DbContextFactory.CreateDbContext(_serviceProvider);
            _DictOfmissions = await dbContext.Missions.Include(m => m.Agent).Include(m => m.Target)
                .ToDictionaryAsync(k => (k.Agent.Id, k.Target.Id), v => v);
            return _DictOfmissions.ContainsKey((agent.Id, target.Id));

        }
        public static double CalculateDistanceDouble(AgentModel agent, TargetModel target)
        {
            double distance =
                Math.Sqrt(Math.Pow(target.Location_x - agent.Location_x, 2)
                + Math.Pow(target.Location_y - agent.Location_y, 2));
            _DictOfDistance[(agent.Id, target.Id)] = distance;
            return distance;
        }


        public static bool CalculateDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2)) < 200;
        }
        public static bool IsValidLocationAgen(AgentModel agent) =>
             agent.Location_x > 0 && agent.Location_y > 0;

        public static bool IsValidLocationTarget(TargetModel target) =>
             target.Location_x > 0 && target.Location_y > 0;
        public async Task<List<MissionModel>> GetAllMissions()
        {
            using var dbContext = DbContextFactory.CreateDbContext(_serviceProvider);
            return await dbContext.Missions
            .Include(m => m.Agent)
        .Include(m => m.Target)
        .ToListAsync();
        }

        public async Task<List<MissionModel>> GetAllMissionswithoutIclude()
        {
            using var dbContext = DbContextFactory.CreateDbContext(_serviceProvider);
            return await dbContext.Missions.ToListAsync();
        }

        public async Task<List<MissionModel>> GetMissionsByStatus(StatusMission status)
        {
            using var dbContext = DbContextFactory.CreateDbContext(_serviceProvider);

            return await dbContext.Missions.Include(m => m.Agent)
        .Include(m => m.Target)
            .Where(m => m.Status == status).ToListAsync();
        }


        public async Task<List<MissionModel>> GetMissionsByStatuswithOutInclude(StatusMission status)
        {
            using var dbContext = DbContextFactory.CreateDbContext(_serviceProvider);

            return await dbContext.Missions
            .Where(m => m.Status == status).ToListAsync();
        }
        public async Task StartTrackink(int idMission)
        {

            using var dbContext = DbContextFactory.CreateDbContext(_serviceProvider);
            MissionModel? mission = await dbContext.Missions.Include(x => x.Agent)
                .Include(m => m.Target).FirstOrDefaultAsync(x => x.Id == idMission);
            if (mission == null)
            {
                throw new Exception($"{idMission} is not found");
            }

            mission.Status = StatusMission.action;
            mission.Agent.Status = StatusAgent.Active;
            mission.Target.Status = StatusTarget.monitoredByAnAgent;
            _stopwatch[mission.Id] = new Stopwatch();
            _stopwatch[mission.Id].Start();

            await dbContext.SaveChangesAsync();
        }

        public async Task GetMissionAtStatusActiveForSteps()
        {
            try
            {
                List<MissionModel> missionsAtStatusActive = await GetMissionsByStatus(StatusMission.action);

                List<Task> tasks = missionsAtStatusActive.Select(StepsTheAnAgentToTarget).ToList();
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
            }
        }
        public async Task StepsTheAnAgentToTarget(MissionModel mission)
        {
            using var dbContext = DbContextFactory.CreateDbContext(_serviceProvider);
            MissionModel? m = await dbContext.Missions.Include(x => x.Agent).Include(x => x.Target)
            .FirstOrDefaultAsync(x => x.Id == mission.Id);
            int agent_x = mission.Agent.Location_x;
            int agent_y = mission.Agent.Location_y;
            int target_x = mission.Target.Location_x;
            int target_y = mission.Target.Location_y;

            int xDirection = agent_x < target_x ? 1 : (agent_x > target_x ? -1 : 0);
            int yDirection = agent_y < target_y ? 1 : (agent_y > target_y ? -1 : 0);

            agent_x += xDirection;
            agent_y += yDirection;

            m.Agent.Location_x = agent_x;
            m.Agent.Location_y = agent_y;
            await dbContext.SaveChangesAsync();
            if (agent_x == target_x && agent_y == target_y)
            {
                await UpdateAtEndOfTheMission(mission.Id);
                return;
            }


        }
        public async Task<MissionModel?> GetMissionById(int id)
        {
            using var dbContext = DbContextFactory.CreateDbContext(_serviceProvider);
            return await dbContext.Missions.Include(x => x.Agent).Include(x => x.Target)
            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAtEndOfTheMission(int id)
        {
            using var dbContext = DbContextFactory.CreateDbContext(_serviceProvider);
            MissionModel? missionAccomplished = await dbContext.Missions
                .Include(x => x.Agent).Include(x => x.Target)
            .FirstOrDefaultAsync(x => x.Id == id);
            if (missionAccomplished == null)
            {
                return;
            }
            missionAccomplished.Status = StatusMission.Actioncompleted;
            missionAccomplished.Target.Status = StatusTarget.Eliminated;
            missionAccomplished.Agent.Status = StatusAgent.Inactive;
            _stopwatch[missionAccomplished.Id].Stop();
            missionAccomplished.ActualExecutionTime = _stopwatch[missionAccomplished.Id].Elapsed.ToString(@"hh\:mm\:ss");
            await dbContext.SaveChangesAsync();
        }
    }
}