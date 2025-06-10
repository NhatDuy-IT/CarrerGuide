using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAspNetCoreApp.Models;

namespace MyAspNetCoreApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly CareerGuideDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardController(CareerGuideDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var dashboardData = new DashboardViewModel
            {
                TotalUsers = await _userManager.Users.CountAsync(),
                TotalSchools = await _context.Schools.CountAsync(),
                TotalExams = await _context.Exams.CountAsync(),
                TotalSurveys = await _context.Surveys.CountAsync(),
                TotalExamResults = await _context.ExamResults.CountAsync(),
                TotalSurveyResults = await _context.SurveyResults.CountAsync(),
                RecentExamResults = await _context.ExamResults
                    .Include(er => er.Exam)
                    .OrderByDescending(er => er.TakenDate)
                    .Take(5)
                    .ToListAsync(),
                RecentSurveyResults = await _context.SurveyResults
                    .Include(sr => sr.Survey)
                    .OrderByDescending(sr => sr.CompletedDate)
                    .Take(5)
                    .ToListAsync()
            };

            return View(dashboardData);
        }
    }

    public class DashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalSchools { get; set; }
        public int TotalExams { get; set; }
        public int TotalSurveys { get; set; }
        public int TotalExamResults { get; set; }
        public int TotalSurveyResults { get; set; }
        public List<ExamResult> RecentExamResults { get; set; } = new();
        public List<SurveyResult> RecentSurveyResults { get; set; } = new();
    }
}
