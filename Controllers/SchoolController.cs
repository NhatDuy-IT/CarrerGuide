using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAspNetCoreApp.Models;

namespace MyAspNetCoreApp.Controllers
{
    public class SchoolController : Controller
    {
        private readonly CareerGuideDbContext _context;

        public SchoolController(CareerGuideDbContext context)
        {
            _context = context;
        }

        // GET: School
        public async Task<IActionResult> Index()
        {
            var schools = await _context.Schools.ToListAsync();
            var schoolDtos = SchoolDto.FromSchoolList(schools);
            return View(schoolDtos);
        }

        // GET: School/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var school = await _context.Schools
                .FirstOrDefaultAsync(m => m.Id == id);

            if (school == null)
            {
                return NotFound();
            }

            var schoolDto = SchoolDto.FromSchool(school);
            return View(schoolDto);
        }

        // GET: School/Search
        public async Task<IActionResult> Search(string searchTerm)
        {
            var schools = await _context.Schools
                .Where(s => string.IsNullOrEmpty(searchTerm) ||
                           s.Name.Contains(searchTerm) ||
                           s.Description.Contains(searchTerm))
                .ToListAsync();

            var schoolDtos = SchoolDto.FromSchoolList(schools);
            ViewBag.SearchTerm = searchTerm;
            return View("Index", schoolDtos);
        }

        // GET: School/Recommendations
        public async Task<IActionResult> Recommendations()
        {
            // Logic for school recommendations based on user preferences
            var recommendedSchools = await _context.Schools
                .OrderBy(s => s.TuitionFee)
                .Take(5)
                .ToListAsync();

            var schoolDtos = SchoolDto.FromSchoolList(recommendedSchools);
            return View(schoolDtos);
        }
    }
}
