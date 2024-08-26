using FotTestApi.Data;
using FotTestApi.ModelDto;
using FotTestApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FotTestApi.Service
{
	public class TargetService : ITargetService
	{
		private readonly ApplicationBDContext _context;
		private readonly IMissionsService _missionsService;
		public TargetService(ApplicationBDContext context, IMissionsService missionsService)
		{
			_context = context;
			_missionsService = missionsService;
		}
		private readonly Dictionary<string, (int x, int y)> stepsByDirection = new()
	{
		{"n", (x: 0,y: 1) },
		{"s", (x: 0,y: -1) },
		{"e", (x: 1,y: 0) },
		{"w", (x: -1,y: 0) },
		{"nw", (x: -1,y: 1) },
		{"ne", (x: 1,y: 1) },
		{"sw", (x: -1,y: -1) },
		{"se", (x: 1,y: -1) }
	};
		public async Task<TargetModel> CreateTargetService(TargetDto targetDto)
		{
			try
			{
				TargetModel newTarget = new()
				{
					Name = targetDto.Name,
					Role = targetDto.Position,
					Image = targetDto.PhotoUrl,
				};
				await _context.Targets.AddAsync(newTarget);
				await _context.SaveChangesAsync();
				return newTarget;
			}
			catch (Exception ex)
			{
				throw new Exception($"{ex.Message}", ex);
			}
		}



		public async Task<TargetModel?> FindTargetByIdService(int id)
		{
			TargetModel? target = await _context.Targets.FindAsync(id);
			if (target is null) { throw new Exception(ErrorMassage.errorNotFound); }
			return target;
		}
		public async Task StartingPositionService(LocationDto location, int id)
		{
			try
			{
				TestLocation(location);
				TargetModel? target = await FindTargetByIdService(id);
				target!.Location_x = location.x;
				target.Location_y = location.y;
				await _context.SaveChangesAsync();
				await _missionsService.CheckIfCreateMission();
			}
			catch (Exception ex)
			{
				throw new Exception($"{ex.Message}", ex);
			}
		}


		public void TestLocation(LocationDto location)
		{
			if (location.x < 0 || location.x > 1000 || location.y < 0 || location.y > 1000)
			{
				throw new Exception(ErrorMassage.errorNumberHighOrLow);
			}
		}

		public async Task<TargetModel?> StepsAgent(DiractionDto diractionDto, int id)
		{
			try
			{
				bool keyExists = stepsByDirection.ContainsKey(diractionDto.direction);
				if (!keyExists)
				{
					throw new Exception(ErrorMassage.notContainsKey);
				}
				var (xForTuple, yForTuple) = stepsByDirection[diractionDto.direction];
				TargetModel? target = await FindTargetByIdService(id);
				switch (target)
				{
				case null:

				return null;

				case { Status: StatusTarget.Eliminated }:

				return null;

				default:
				TestLocation(new()
				{
					x = xForTuple + target!.Location_x,
					y = yForTuple + target.Location_y
				});
				target.Location_x += xForTuple;
				target.Location_y += yForTuple;
				await _context.SaveChangesAsync();
				await _missionsService.CheckIfCreateMission();
				return target;

				}

			}
			catch (Exception ex)
			{
				throw new Exception($"{ex.Message}", ex);
			}
		}
		public async Task<List<TargetModel>?> GetAllAgent() =>
		await _context.Targets.ToListAsync();
	}
}