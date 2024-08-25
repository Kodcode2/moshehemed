using FotTestApi.ModelDto;
using FotTestApi.Models;

namespace FotTestApi.Service
{
	public interface ITargetService
	{
		Task<TargetModel> CreateTargetService(TargetDto targetDto);
		Task StartingPositionService(LocationDto location, int id);
		Task<TargetModel?> FindTargetByIdService(int id);
		Task<TargetModel?> StepsAgent(DiractionDto diractionDto, int id);
		Task<List<TargetModel>?> GetAllAgent();
	}
}
