using Microsoft.AspNetCore.Mvc;

namespace Fitness_Tracker.Controllers
{
    public class GenerateController : Controller
    {
        public IActionResult Index()
        {
            return View("Generate");
        }

        [HttpPost]
        public IActionResult Results(int age, int feet, int inches, int weight, string gender, int goal, int activity)
        {
            ViewBag.Age = age;
            ViewBag.Feet = feet;
            ViewBag.Inches = inches;
            ViewBag.Weight = weight;
            ViewBag.Gender = gender;
            ViewBag.Goal = goal;
            ViewBag.Activity = activity;
            
            return View("Results");
        }
    }
}
