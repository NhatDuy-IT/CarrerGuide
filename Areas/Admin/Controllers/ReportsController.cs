using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAspNetCoreApp.Models;

namespace MyAspNetCoreApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly CareerGuideDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ReportsController(CareerGuideDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/Reports
        public async Task<IActionResult> Index()
        {
            var report = new SystemReportViewModel
            {
                TotalUsers = await _userManager.Users.CountAsync(),
                TotalSchools = await _context.Schools.CountAsync(),
                TotalExams = await _context.Exams.CountAsync(),
                TotalSurveys = await _context.Surveys.CountAsync(),
                TotalExamResults = await _context.ExamResults.CountAsync(),
                TotalSurveyResults = await _context.SurveyResults.CountAsync(),
                
                // User registrations by month (last 6 months)
                UserRegistrationsByMonth = await GetUserRegistrationsByMonth(),
                
                // Exam performance statistics
                ExamPerformanceStats = await GetExamPerformanceStats(),
                
                // Popular schools
                PopularSchools = await GetPopularSchools(),
                
                // Recent activities
                RecentActivities = await GetRecentActivities()
            };

            return View(report);
        }

        // GET: Admin/Reports/UserActivity
        public async Task<IActionResult> UserActivity()
        {
            var userActivities = await _context.ExamResults
                .Include(er => er.Exam)
                .GroupBy(er => er.UserId)
                .Select(g => new UserActivityViewModel
                {
                    UserId = g.Key,
                    ExamsTaken = g.Count(),
                    AverageScore = g.Average(er => er.Score),
                    LastActivity = g.Max(er => er.TakenDate)
                })
                .OrderByDescending(ua => ua.LastActivity)
                .Take(50)
                .ToListAsync();

            // Get user emails
            foreach (var activity in userActivities)
            {
                var user = await _userManager.FindByIdAsync(activity.UserId);
                activity.UserEmail = user?.Email ?? "Unknown";
            }

            return View(userActivities);
        }

        // GET: Admin/Reports/ExamStatistics
        public async Task<IActionResult> ExamStatistics()
        {
            var examStats = await _context.Exams
                .Select(e => new ExamStatisticsViewModel
                {
                    ExamId = e.Id,
                    ExamTitle = e.Title,
                    TotalAttempts = e.ExamResults.Count(),
                    AverageScore = e.ExamResults.Any() ? e.ExamResults.Average(er => er.Score) : 0,
                    PassRate = e.ExamResults.Any() ? 
                        (double)e.ExamResults.Count(er => er.Score >= e.PassingScore) / e.ExamResults.Count() * 100 : 0,
                    HighestScore = e.ExamResults.Any() ? e.ExamResults.Max(er => er.Score) : 0,
                    LowestScore = e.ExamResults.Any() ? e.ExamResults.Min(er => er.Score) : 0
                })
                .ToListAsync();

            return View(examStats);
        }

        private async Task<List<MonthlyDataViewModel>> GetUserRegistrationsByMonth()
        {
            var sixMonthsAgo = DateTime.Now.AddMonths(-6);
            var users = await _userManager.Users
                .Where(u => u.LockoutEnd == null || u.LockoutEnd < DateTimeOffset.Now)
                .ToListAsync();

            return Enumerable.Range(0, 6)
                .Select(i => DateTime.Now.AddMonths(-i))
                .Select(month => new MonthlyDataViewModel
                {
                    Month = month.ToString("MM/yyyy"),
                    Count = users.Count() // Simplified - in real app, you'd track registration dates
                })
                .Reverse()
                .ToList();
        }

        private async Task<List<ExamPerformanceViewModel>> GetExamPerformanceStats()
        {
            return await _context.Exams
                .Select(e => new ExamPerformanceViewModel
                {
                    ExamTitle = e.Title,
                    TotalAttempts = e.ExamResults.Count(),
                    AverageScore = e.ExamResults.Any() ? e.ExamResults.Average(er => er.Score) : 0,
                    PassRate = e.ExamResults.Any() ? 
                        (double)e.ExamResults.Count(er => er.Score >= e.PassingScore) / e.ExamResults.Count() * 100 : 0
                })
                .OrderByDescending(e => e.TotalAttempts)
                .Take(10)
                .ToListAsync();
        }

        private async Task<List<PopularSchoolViewModel>> GetPopularSchools()
        {
            return await _context.Schools
                .Select(s => new PopularSchoolViewModel
                {
                    SchoolName = s.Name,
                    ViewCount = 0, // Simplified - in real app, you'd track views
                    Location = s.Address // Use Address instead of Location
                })
                .OrderBy(s => s.SchoolName)
                .Take(10)
                .ToListAsync();
        }

        private async Task<List<RecentActivityViewModel>> GetRecentActivities()
        {
            var examActivities = await _context.ExamResults
                .Include(er => er.Exam)
                .OrderByDescending(er => er.TakenDate)
                .Take(10)
                .Select(er => new RecentActivityViewModel
                {
                    Type = "Exam",
                    Description = $"Completed exam: {er.Exam.Title}",
                    UserId = er.UserId,
                    Timestamp = er.TakenDate
                })
                .ToListAsync();

            return examActivities;
        }
    }

    // ViewModels for Reports
    public class SystemReportViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalSchools { get; set; }
        public int TotalExams { get; set; }
        public int TotalSurveys { get; set; }
        public int TotalExamResults { get; set; }
        public int TotalSurveyResults { get; set; }
        public List<MonthlyDataViewModel> UserRegistrationsByMonth { get; set; } = new();
        public List<ExamPerformanceViewModel> ExamPerformanceStats { get; set; } = new();
        public List<PopularSchoolViewModel> PopularSchools { get; set; } = new();
        public List<RecentActivityViewModel> RecentActivities { get; set; } = new();
    }

    public class MonthlyDataViewModel
    {
        public string Month { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class ExamPerformanceViewModel
    {
        public string ExamTitle { get; set; } = string.Empty;
        public int TotalAttempts { get; set; }
        public double AverageScore { get; set; }
        public double PassRate { get; set; }
    }

    public class PopularSchoolViewModel
    {
        public string SchoolName { get; set; } = string.Empty;
        public int ViewCount { get; set; }
        public string Location { get; set; } = string.Empty;
    }

    public class RecentActivityViewModel
    {
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }

    public class UserActivityViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public int ExamsTaken { get; set; }
        public double AverageScore { get; set; }
        public DateTime LastActivity { get; set; }
    }

    public class ExamStatisticsViewModel
    {
        public int ExamId { get; set; }
        public string ExamTitle { get; set; } = string.Empty;
        public int TotalAttempts { get; set; }
        public double AverageScore { get; set; }
        public double PassRate { get; set; }
        public double HighestScore { get; set; }
        public double LowestScore { get; set; }
    }
}
