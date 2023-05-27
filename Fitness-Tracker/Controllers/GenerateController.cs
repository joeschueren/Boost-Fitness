using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
            double bmr;
            int totalInches = (feet * 12) +inches;
            if(gender == "male")
            {
                bmr = 66 + (6.23 * weight) + (12.7 * totalInches) - (6.8 * age);
            }
            else
            {
                bmr = 665 + (4.35 * weight) + (4.7 * totalInches) - (4.7 * age);
            }

            double tcb;
            if(activity == 0) 
            {
                tcb = bmr * 1.2;
            }
            else if(activity == 1)
            {
                tcb = bmr * 1.375;
            }
            else if(activity == 2)
            {
                tcb = bmr * 1.55;
            }
            else
            {
                tcb = bmr * 1.725;
            }


            bool[] generateSchedule(int activity)
            {
                bool[] returnArray = {false, false, false, false, false, false, false};

                if (activity == 1) 
                {
                    for(int i = 0; i < returnArray.Length; i++)
                    {
                        if (i == 0 || i == 3 || i == 6)
                        {
                            returnArray[i] = true;
                        }
                        
                    } 
                }

                if (activity == 2)
                {
                    for (int i = 0; i < returnArray.Length; i++)
                    {
                        if (i == 0 || i ==1 || i == 3 || i == 4 || i == 6)
                        {
                            returnArray[i] = true;
                        }

                    }
                }

                if (activity == 3)
                {
                    for (int i = 0; i < returnArray.Length; i++)
                    {
                        returnArray[i] = true;
                    }
                }
                    return returnArray;
            }

            bool[] exercisePlan = generateSchedule(activity);

            int caloriesPerDay = (int) ((tcb * 7) - (1750 * goal)) / 7;

            int caloriesExpected = (int)tcb - caloriesPerDay;

            string[] daysArray = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            string currentWeekday = DateTime.Now.DayOfWeek.ToString();
            int index = Array.IndexOf(daysArray, currentWeekday);
            string[] days = new string[daysArray.Length];
            for (int i = 0; i< 7; i++)
            {
                days[i] = daysArray[index];
                if(index == 6)
                {
                    index = 0;
                }
                else
                {
                    index++;
                }
            }

            Dictionary<object, object> day1 = new Dictionary<object, object>();

            day1["calories"] = caloriesPerDay;
            day1["exercise"] = exercisePlan[0];
            day1["burned"] = caloriesExpected;
            day1["day"] = days[0];

            ViewBag.day1 = day1;

            Dictionary<object, object> day2 = new Dictionary<object, object>();

            day2["calories"] = caloriesPerDay;
            day2["exercise"] = exercisePlan[1];
            day2["burned"] = caloriesExpected;
            day2["day"] = days[1];

            ViewBag.day2 = day2;

            Dictionary<object, object> day3 = new Dictionary<object, object>();

            day3["calories"] = caloriesPerDay;
            day3["exercise"] = exercisePlan[2];
            day3["burned"] = caloriesExpected;
            day3["day"] = days[2];

            ViewBag.day3 = day3;

            Dictionary<object, object> day4 = new Dictionary<object, object>();

            day4["calories"] = caloriesPerDay;
            day4["exercise"] = exercisePlan[3];
            day4["burned"] = caloriesExpected;
            day4["day"] = days[3];

            ViewBag.day4 = day4;

            Dictionary<object, object> day5 = new Dictionary<object, object>();

            day5["calories"] = caloriesPerDay;
            day5["exercise"] = exercisePlan[4];
            day5["burned"] = caloriesExpected;
            day5["day"] = days[4];

            ViewBag.day5 = day5;

            Dictionary<object, object> day6 = new Dictionary<object, object>();

            day6["calories"] = caloriesPerDay;
            day6["exercise"] = exercisePlan[5];
            day6["burned"] = caloriesExpected;
            day6["day"] = days[5];

            ViewBag.day6 = day6;

            Dictionary<object, object> day7 = new Dictionary<object, object>();

            day7["calories"] = caloriesPerDay;
            day7["exercise"] = exercisePlan[6];
            day7["burned"] = caloriesExpected;
            day7["day"] = days[6];

            ViewBag.day7 = day7;

            int hoursLeft;

            hoursLeft = 23 - DateTime.Now.Hour;

            ViewBag.hoursLeft = hoursLeft;

            int ozWater = (int)(weight * .5);

            ViewBag.ozWater = ozWater;

            return View("Results");
        }
    }
}
