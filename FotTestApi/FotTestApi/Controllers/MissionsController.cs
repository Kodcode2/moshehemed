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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [HttpPut("start/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> StartMission(int id)
        {
            try
            {
                await missionsService.StartTrackink(id);
                return Ok("Good");
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<MissionModel>>> GetAllMissins()
        {
            try
            {
                List<MissionModel> allMissions = await missionsService.GetAllMissionswithoutIclude();
                return Ok(allMissions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("missionsByStatus/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<MissionModel>>> GetMissinsByStatus(StatusMission status)
        {
            try
            {
                List<MissionModel> missions = await missionsService.GetMissionsByStatuswithOutInclude(status);
                return Ok(missions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}

