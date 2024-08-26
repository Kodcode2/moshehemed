using FotTestApi.Models;
using FotTestApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FotTestApi.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class MissionsController(IMissionsService missionsService) : ControllerBase
	{
		[HttpPost("update")]
		public async Task<ActionResult> StepTowardsTheTarget()
		{
			try
			{
				await missionsService.GetMissionAtStatusActiveForSteps();
				return Ok("Missions in active status have moved towards the target");
			
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}
		[HttpPut("PUT/missions/start/{id}")]
		public async Task<ActionResult> StartMission(int id)
		{
			try
			{
				await missionsService.StartTrackink(id);
				return Ok("Good");
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

	}
}
