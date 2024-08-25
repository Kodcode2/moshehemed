using FotTestApi.ModelDto;
using FotTestApi.Models;
using FotTestApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace FotTestApi.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class TargetsController(ITargetService targetService) : ControllerBase
	{
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<AgentDto>> CreateAgent([FromBody] TargetDto targetDto)
		{

			try
			{
				TargetModel newTarget = await targetService.CreateTargetService(targetDto);
				return Created("Created successfully", new { Id = newTarget.Id });
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		[HttpPut("{id}/pin")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> StartingPosition([FromBody] LocationDto locationTarget, int id)
		{
			try
			{
				await targetService.StartingPositionService(locationTarget, id);
				return Ok("Update successfully");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		[HttpPut("{id}/move")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> StepsAgent([FromBody] DiractionDto diractionDto, int id)
		{
			try
			{
				TargetModel? agent = await targetService.StepsAgent(diractionDto, id);
				return Ok($"The proof position is x= {agent?.Location_x}, y = {agent?.Location_y}");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult> GetAllAgents()
		{
			List<TargetModel>? a = await targetService.GetAllAgent();
			return Ok(a);
		}
	}
}
