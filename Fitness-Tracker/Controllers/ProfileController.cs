using Fitness_Tracker.Data;
using Fitness_Tracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitness_Tracker.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AlternativeDbContext _context;

        public ProfileController(AlternativeDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            string username = User.Identity.Name;

            var user = _context.Users.FirstOrDefault(x => x.User == username);

            ViewBag.user = user;

            var days = _context.Days.Where(x => x.User == username).ToList();
               
            ViewBag.days = days;

            _context.Dispose();

            return View();
        }

        [HttpPost]
        public IActionResult Input(string User, int weight, int feet, int inches,  int age, string gender)
        {
            int height = (feet * 12) + inches;

            UserInfo newUser = new UserInfo
            {
                User = User,
                Weight = weight,
                Height = height,
                Age = age,
                Gender = gender,
                TableReady = true

            };

            _context.Users.Add(newUser);

            _context.Dispose();

            return RedirectToAction("Index", "Profile");
        }
    }
}
