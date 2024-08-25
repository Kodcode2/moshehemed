using FotTestApi.ModelDto;
using FotTestApi.Models;
using FotTestApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FotTestApi.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AgentsController(IAgenService agenService) : ControllerBase
	{
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<AgentDto>> CreateAgent([FromBody] AgentDto agentDto)
		{
			try
			{
				AgentModel newAgent = await agenService.CreateAsyncService(agentDto);
				return Created("Created successfully", new { Id = newAgent.Id });
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

		}
		[HttpPut("{id}/pin")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> StartingPosition([FromBody] LocationDto locationAgent, int id)
		{ 
			try
			{
				await agenService.StartingPositionService(locationAgent, id);
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
		public async Task<ActionResult> StepsAgent([FromBody] string diraction, int id)
		{
			var a = 8;
			try
			{
				AgentModel? agent = await agenService.StepsAgent(diraction, id);
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
			List<AgentModel>? a=  await agenService.GetAllAgent();
			return Ok(a);

		}
		
	}
}