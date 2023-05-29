using Fitness_Tracker.Data;
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
            return View();
        }
    }
}
