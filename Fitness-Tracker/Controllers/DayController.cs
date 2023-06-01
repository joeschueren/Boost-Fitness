using Fitness_Tracker.Data;
using Fitness_Tracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace Fitness_Tracker.Controllers
{
    public class DayController : Controller
    {
        private readonly AlternativeDbContext _context;

        public DayController(AlternativeDbContext context)
        {
            _context = context;
        }
        public IActionResult Add()
        {

            string[] dates = new string[7];

            DateTime currentDate = DateTime.Now;

            for (int i = 0; i< dates.Length; i++)
            {
                int day = currentDate.Day;
                int month = currentDate.Month;
                int year = currentDate.Year;

                string date = $"{month}/{day}/{year}";

                dates[i] = date;

                currentDate = currentDate.AddDays(-1);
            }

            ViewBag.dates = dates;

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Insert(string user, int minutes, int calories, string date)
        {
            bool AlreadyInput = _context.Days.Any(d => d.User == user && d.Date == date);
            if (AlreadyInput)
            {
                var day = _context.Days.FirstOrDefault(d => d.User == user && d.Date == date);

                day.CaloriesIn = calories;

                day.MinExercise = minutes;

                _context.SaveChanges();

                _context.Dispose();
            }
            else {
                Day newDay = new Day
                {
                    User = user,
                    CaloriesIn = calories,
                    MinExercise = minutes,
                    Date = date,
                    TableReady = true
                };

                _context.Days.Add(newDay);

                _context.SaveChanges();

                _context.Dispose();
            }

            return RedirectToAction("Index", "Profile");
        }
    }
}
