using Microsoft.AspNetCore.Mvc;
using MVCTest.Models;
using MVCTest.Services;

namespace MVCTest.Controllers
{
    public class TargetController : Controller
    {
        private readonly ITargetService _targetService;
        public TargetController(ITargetService targetService)
        {
            _targetService = targetService;
        }
        public async Task<IActionResult> Index()
        {
            List<TargetModel>? targets = await _targetService.GetAllAgentsAsync();
            if (targets != null)
            {
                return View(targets);
            }
            return RedirectToAction("Index", "Home");

        }
    }
}
