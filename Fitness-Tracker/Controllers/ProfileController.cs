using Fitness_Tracker.Data;
using Fitness_Tracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Humanizer.In;
using System.Reflection;
using System.Diagnostics;

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

            if(user == null)
            {
                return View();
            }

            var days = _context.Days.Where(x => x.User == username).ToList();
               
            ViewBag.days = days;

            DateTime today = DateTime.Today;
            int daysUntilSunday = (0 - (int)today.DayOfWeek) % 7;
           

            double bmr;
            int totalInches = user.Height;
            string gender = user.Gender;
            int weight = user.Weight; 
            int age = user.Age;
            int totalMinutes = 0;
            int activity = 0;
            foreach(var day in days)
            {
                totalMinutes += day.MinExercise;
            }

            if(totalMinutes < (90 / (int)today.DayOfWeek))
            {
                activity = 1;
            }
            else if(totalMinutes < (150 / (int)today.DayOfWeek))
            {
                activity = 2;
            }
            else if(totalMinutes < (180 / (int)today.DayOfWeek))
            {
                activity = 3;
            }

            if (gender == "male")
            {
                bmr = 66 + (6.23 * weight) + (12.7 * totalInches) - (6.8 * age);
            }
            else
            {
                bmr = 665 + (4.35 * weight) + (4.7 * totalInches) - (4.7 * age);
            }

            double tcb;
            if (activity == 0)
            {
                tcb = bmr * 1.2;
            }
            else if (activity == 1)
            {
                tcb = bmr * 1.375;
            }
            else if (activity == 2)
            {
                tcb = bmr * 1.55;
            }
            else
            {
                tcb = bmr * 1.725;
            }

            int totalBurned = 0;
            foreach(var day in days)
            {
                totalBurned += Math.Abs(day.CaloriesIn - (int)tcb);
            }

            Dictionary<string, object>[] daysData = new Dictionary<string, object>[7];

            int index = 0;

            for (int i = daysUntilSunday; i <  7 + daysUntilSunday; i++)
            {
                var dayData = new Dictionary<string, object>();

                var currentDay = days.FirstOrDefault(d => d.Date == today.AddDays(i).ToString("MM/dd/yyyy"));
                var dayOfWeek = today.AddDays(i).DayOfWeek;

                int updatedExpected = (7000 - totalBurned) / (7 - (int)today.DayOfWeek);
                int updatedIntake = (int)tcb - updatedExpected;

                if (currentDay == null && dayOfWeek <= today.DayOfWeek)
                {
                    dayData["calories"] = (int)Math.Round(tcb);
                    dayData["minutes"] = 0;
                    dayData["burned"] = 0;
                    dayData["expected"] = 1000;
                    dayData["where"] = 1;
                }
                else if (dayOfWeek <= today.DayOfWeek)
                {
                    dayData["calories"] = (int)Math.Round((double)currentDay.CaloriesIn);
                    dayData["minutes"] = currentDay.MinExercise;
                    dayData["burned"] = (int)Math.Round(currentDay.CaloriesIn - tcb);
                    dayData["expected"] = 1000;
                    dayData["where"] = 2;
                }
                else
                {
                    dayData["calories"] = updatedIntake;
                    dayData["minutes"] = 30;
                    dayData["burned"] = updatedExpected;
                    dayData["expected"] = updatedExpected;
                    dayData["where"] = 3;
                }

                daysData[index] = dayData;
                index++;

            }

            ViewBag.dayOfWeek = today.DayOfWeek;

            ViewBag.daysData = daysData;

            _context.Dispose();

            return View();
        }

        [HttpPost]
        [Authorize]
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

            _context.SaveChanges();

            _context.Dispose();

            return RedirectToAction("Index", "Profile");
        }

        [Authorize]
        public IActionResult Update()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Updateinfo(int weight, int feet, int inches, int age, string gender) 
        {
            string username = User.Identity.Name;

            int height = (feet * 12) + inches;

            var user = _context.Users.FirstOrDefault(x => x.User == username);

            if(user != null)
            {
                user.Weight = weight;
                user.Height = height;
                user.Age = age;
                user.Gender = gender;
                user.TableReady = true;

                _context.Users.Add(user);

                _context.SaveChanges();

                _context.Dispose();

                return RedirectToAction("Index", "Profile");
            }
            else
            {
                return RedirectToAction("Index", "Profile");
            }

            
        }


    }
}
