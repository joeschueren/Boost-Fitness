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

            int caloriesPerDay = (int) (tcb * 7) - (1750 * goal);

            Dictionary<object, object> day1 = new Dictionary<object, object>();

            day1.Add("calories", caloriesPerDay);
            
            return View("Results");
        }
    }
}
