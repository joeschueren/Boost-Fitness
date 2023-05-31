using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitness_Tracker.Controllers
{
    public class DayController : Controller
    {
        public IActionResult Add()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Insert(int minutes, int calories, string date)
        { 
            return RedirectToAction("Index", "Profile");
        }
    }
}
