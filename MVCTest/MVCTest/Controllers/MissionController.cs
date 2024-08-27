using Microsoft.AspNetCore.Mvc;
using MVCTest.Models;
using MVCTest.Services;

namespace MVCTest.Controllers
{

	public class MissionController : Controller
	{
		private readonly IMissionService _missionService;
		public MissionController(IMissionService missionService)
		{
			_missionService = missionService;

		}
		public async Task<IActionResult> Index()
		{
			List<MissionModel>? agents = await _missionService.GetAllMissionsAsync();
			if (agents != null)
			{
				return View(agents);
			}
			return RedirectToAction("Index", "Home");
		}
		public async Task<IActionResult> SelectMissions(int id)
		{
			try
			{
				await _missionService.SelectMissions(id);
				return RedirectToAction("Index");
			}
			catch
			{
				return RedirectToAction("Index", "Home");
			}

			}
	}

}