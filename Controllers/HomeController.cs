using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAspNetCoreApp.Models;
using System.Diagnostics;

namespace MyAspNetCoreApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly CareerGuideDbContext _context;

        public HomeController(CareerGuideDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Lấy 20 trường có học phí cao nhất, sau đó random 10 trường
            var topExpensiveSchools = await _context.Schools
                .OrderByDescending(s => s.TuitionFee)
                .Take(20)
                .ToListAsync();

            // Random 10 trường từ 20 trường có học phí cao nhất
            var random = new Random();
            var randomSchools = topExpensiveSchools
                .OrderBy(x => random.Next())
                .Take(10)
                .ToList();

            // Convert to DTO
            var schoolDtos = SchoolDto.FromSchoolList(randomSchools);

            return View(schoolDtos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorViewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = HttpContext.Response.StatusCode
            };

            return View(errorViewModel);
        }

        public IActionResult MbtiAssessment()
        {
            return View();
        }

        public IActionResult SchoolRecommendations()
        {
            return View();
        }
    }
}