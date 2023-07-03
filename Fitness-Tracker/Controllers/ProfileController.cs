using Fitness_Tracker.Data;
using Fitness_Tracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Humanizer.In;
using System.Reflection;
using System.Diagnostics;
using Fitness_Tracker.Migrations;
using System.Data.SqlTypes;

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

            var stats = _context.Stats.FirstOrDefault(x => x.User == username);

            ViewBag.user = user;

            ViewBag.stats = stats;

            if(user == null || stats == null)
            {
                return View();
            }

            var days = _context.Days.Where(x => x.User == username).ToList();
               
            ViewBag.days = days;

            int hoursLeft = 23 - DateTime.Now.Hour;

            ViewBag.hoursLeft = hoursLeft;

            DateTime today = DateTime.Today;
            int daysUntilSunday = (0 - (int)today.DayOfWeek) % 7;

            var sunday = today.AddDays(daysUntilSunday);

            string[] datesList = new string[7];

            for(int i =0; i < 7; i++)
            {
                datesList[i] = sunday.AddDays(i).ToString("MM/dd/yyyy");
            }
           

            double bmr;
            int totalInches = user.Height;
            string gender = user.Gender;
            int weight = user.Weight; 
            int age = user.Age;
            int totalMinutes = 0;
            int activity = 0;

            int accumulativeMinutes = 0;
            foreach(var day in days)
            {
                if(datesList.Any(d => d == day.Date))
                {
                    totalMinutes += day.MinExercise;
                }
                accumulativeMinutes += day.MinExercise;
            }

            stats.TotalMinutes = accumulativeMinutes;

            if(totalMinutes < (90 / ((int)today.DayOfWeek + 1)))
            {
                activity = 1;
            }
            else if(totalMinutes < (150 / ((int)today.DayOfWeek + 1)))
            {
                activity = 2;
            }
            else if(totalMinutes < (210 / ((int)today.DayOfWeek + 1)))
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
            int accumulativeBurned = 0;
            foreach(var current in days)
            {
                if(datesList.Any(d => d == current.Date.ToString()))
                {
                    totalBurned += current.CaloriesIn - (int)tcb;
                }
                accumulativeBurned += current.CaloriesIn - (int)tcb;
            }

            stats.TotalBurned = accumulativeBurned;

            _context.SaveChanges();

            

            Dictionary<string, object>[] daysData = new Dictionary<string, object>[7];

            int index = 0;

            for (int i = daysUntilSunday; i <  7 + daysUntilSunday; i++)
            {
                var dayData = new Dictionary<string, object>();

                var currentDay = days.FirstOrDefault(d => d.Date == today.AddDays(i).ToString("MM/dd/yyyy"));
                var dayOfWeek = today.AddDays(i).DayOfWeek;


                int updatedMinutes = 0;
                if(stats.ActivityLevel == 1)
                {
                    updatedMinutes = (90 - totalMinutes) / (7 - (int)today.DayOfWeek + 1);
                }
                else if(stats.ActivityLevel == 2)
                {
                    updatedMinutes = (150 - totalMinutes) / (7 - (int)today.DayOfWeek + 1 );
                }
                else if(stats.ActivityLevel == 3)
                {
                    updatedMinutes = (210 - totalMinutes) / (7 - (int)today.DayOfWeek + 1);
                }
                int updatedExpected = (7000 + totalBurned) / (7 - (int)today.DayOfWeek);
                int updatedIntake = (int)tcb - updatedExpected;



                if (currentDay == null && dayOfWeek <= today.DayOfWeek)
                {
                    dayData["calories"] = (int)Math.Round(tcb);
                    dayData["minutes"] = 0;
                    dayData["burned"] = 0;
                    dayData["expected"] = 250 * stats.Goal;
                    dayData["style"] = "past-box";
                }
                else if (dayOfWeek <= today.DayOfWeek)
                {
                    dayData["calories"] = (int)Math.Round((double)currentDay.CaloriesIn);
                    dayData["minutes"] = currentDay.MinExercise;
                    dayData["burned"] = (int)Math.Round(currentDay.CaloriesIn - tcb);
                    dayData["expected"] = 250 * stats.Goal;
                    dayData["style"] = "past-box";
                }
                else
                {
                    dayData["calories"] = updatedIntake;
                    dayData["minutes"] = updatedMinutes;
                    dayData["burned"] = "n/a";
                    dayData["expected"] = updatedExpected;
                    dayData["style"] = "future-box";
                }

                daysData[index] = dayData;
                index++;

            }

            ViewBag.dayOfWeek = today.DayOfWeek;

            ViewBag.daysData = daysData;

            ViewBag.ozWater = user.Weight / 2;

            _context.Dispose();

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Input(string User, int weight, int feet, int inches,  int age, string gender, int activity, int goal)
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

            Stat newStats = new Stat
            {
                User = User,
                TotalBurned = 0,
                TotalMinutes = 0,
                ActivityLevel = activity,
                Goal = goal,
                TableReady = true


            };

            _context.Users.Add(newUser);

            _context.Stats.Add(newStats);

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
        public IActionResult Updateinfo(int weight, int feet, int inches, int age, string gender, int activity, int goal) 
        {
            string username = User.Identity.Name;

            int height = (feet * 12) + inches;

            var user = _context.Users.FirstOrDefault(x => x.User == username);

            var stats = _context.Stats.FirstOrDefault(x => x.User == username);


            if(user != null && stats != null)
            {
                user.Weight = weight;
                user.Height = height;
                user.Age = age;
                user.Gender = gender;
                user.TableReady = true;

                stats.ActivityLevel = activity;
                stats.Goal = goal;

                _context.SaveChanges();

                

                return RedirectToAction("Index", "Profile");
            }
            else
            {
                return RedirectToAction("Index", "Profile");
            }

            
        }


    }
}
