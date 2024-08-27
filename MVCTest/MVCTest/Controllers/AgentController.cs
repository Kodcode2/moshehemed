using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCTest.Models;
using MVCTest.Services;

namespace MVCTest.Controllers
{

    public class AgentController : Controller
    {
        private readonly IAgentService _agentService;
        public AgentController(IAgentService agentService)
        {
            _agentService = agentService;
        }


        public async Task<IActionResult> Index()
        {
            List<AgentModel>? agents = await _agentService.GetAllAgentsAsync();
            if (agents != null)
            {
                return View(agents);
            }
            return RedirectToAction("Index", "Home");

        }
        public async Task<IActionResult> GetMissionByIdAgent(int id)
        {
            MissionModel? mission = await _agentService.GetMissionByIdAgent(id);
            if (mission != null)
            {
                return View(mission);
            }
            return RedirectToAction("Index");
        }
    }
}
