using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MyAspNetCoreApp.Models;

namespace MyAspNetCoreApp.Controllers
{
    public class SurveyController : Controller
    {
        private readonly CareerGuideDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SurveyController(CareerGuideDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Survey
        public async Task<IActionResult> Index()
        {
            var surveys = await _context.Surveys
                .Where(s => s.IsActive)
                .OrderByDescending(s => s.CreatedDate)
                .ToListAsync();
            
            return View(surveys);
        }

        // GET: Survey/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var survey = await _context.Surveys
                .Include(s => s.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (survey == null)
            {
                return NotFound();
            }

            return View(survey);
        }

        // GET: Survey/Take/5
        public async Task<IActionResult> Take(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var survey = await _context.Surveys
                .Include(s => s.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);

            if (survey == null)
            {
                return NotFound();
            }

            // Order questions and answers
            survey.Questions = survey.Questions.OrderBy(q => q.OrderNumber).ToList();
            foreach (var question in survey.Questions)
            {
                question.Answers = question.Answers.OrderBy(a => a.OrderNumber).ToList();
            }

            return View(survey);
        }

        // POST: Survey/Submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(int surveyId, Dictionary<int, int> answers)
        {
            if (!User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var survey = await _context.Surveys
                .Include(s => s.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(s => s.Id == surveyId);

            if (survey == null)
            {
                return NotFound();
            }

            // Calculate MBTI result
            var personalityScores = new Dictionary<string, int>
            {
                {"E", 0}, {"I", 0}, {"S", 0}, {"N", 0},
                {"T", 0}, {"F", 0}, {"J", 0}, {"P", 0}
            };

            int totalScore = 0;

            foreach (var question in survey.Questions)
            {
                if (answers.ContainsKey(question.Id))
                {
                    var selectedAnswerId = answers[question.Id];
                    var selectedAnswer = question.Answers.FirstOrDefault(a => a.Id == selectedAnswerId);
                    
                    if (selectedAnswer != null)
                    {
                        personalityScores[selectedAnswer.PersonalityType] += selectedAnswer.Points;
                        totalScore += selectedAnswer.Points;
                    }
                }
            }

            // Determine MBTI type
            string mbtiResult = "";
            mbtiResult += personalityScores["E"] > personalityScores["I"] ? "E" : "I";
            mbtiResult += personalityScores["S"] > personalityScores["N"] ? "S" : "N";
            mbtiResult += personalityScores["T"] > personalityScores["F"] ? "T" : "F";
            mbtiResult += personalityScores["J"] > personalityScores["P"] ? "J" : "P";

            // Get description for MBTI type
            string description = GetMBTIDescription(mbtiResult);

            // Save survey result
            var surveyResult = new SurveyResult
            {
                UserId = user.Id,
                SurveyId = surveyId,
                Result = mbtiResult,
                Description = description,
                CompletedDate = DateTime.Now,
                TotalScore = totalScore
            };

            _context.SurveyResults.Add(surveyResult);
            await _context.SaveChangesAsync();

            return RedirectToAction("Result", new { id = surveyResult.Id });
        }

        // GET: Survey/Result/5
        public async Task<IActionResult> Result(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _context.SurveyResults
                .Include(sr => sr.Survey)
                .FirstOrDefaultAsync(sr => sr.Id == id);

            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        // GET: Survey/MyResults
        public async Task<IActionResult> MyResults()
        {
            if (!User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var results = await _context.SurveyResults
                .Include(sr => sr.Survey)
                .Where(sr => sr.UserId == user.Id)
                .OrderByDescending(sr => sr.CompletedDate)
                .ToListAsync();

            return View(results);
        }

        private string GetMBTIDescription(string mbtiType)
        {
            var descriptions = new Dictionary<string, string>
            {
                {"INTJ", "Kiến trúc sư - Người có tầm nhìn chiến lược, sáng tạo và quyết tâm"},
                {"INTP", "Nhà tư duy - Người yêu thích lý thuyết và kiến thức"},
                {"ENTJ", "Chỉ huy viên - Lãnh đạo táo bạo, giàu trí tưởng tượng"},
                {"ENTP", "Nhà tranh luận - Người thông minh, tò mò và không thể cưỡng lại thử thách"},
                {"INFJ", "Người ủng hộ - Người có lý tưởng sáng tạo và nguyên tắc"},
                {"INFP", "Người hòa giải - Người thơ mộng, tử tế và vị tha"},
                {"ENFJ", "Người chủ xướng - Lãnh đạo truyền cảm hứng, quyến rũ và vị tha"},
                {"ENFP", "Người vận động - Người nhiệt tình, sáng tạo và xã hội"},
                {"ISTJ", "Người hậu cần - Người thực tế và tập trung vào sự thật"},
                {"ISFJ", "Người bảo vệ - Người ấm áp và tận tâm, luôn sẵn sàng bảo vệ người thân"},
                {"ESTJ", "Người điều hành - Người quản lý xuất sắc, không thể sánh bằng trong việc quản lý mọi thứ"},
                {"ESFJ", "Người lãnh sự - Người quan tâm, xã hội và hòa đồng"},
                {"ISTP", "Người thợ máy - Người thực nghiệm táo bạo và thực tế"},
                {"ISFP", "Người phiêu lưu - Nghệ sĩ linh hoạt và quyến rũ"},
                {"ESTP", "Người kinh doanh - Người thông minh, năng động và nhạy bén"},
                {"ESFP", "Người giải trí - Người tự phát, năng động và nhiệt tình"}
            };

            return descriptions.ContainsKey(mbtiType) ? descriptions[mbtiType] : "Mô tả không có sẵn";
        }
    }
}
