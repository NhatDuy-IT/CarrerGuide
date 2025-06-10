using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MyAspNetCoreApp.Models;

namespace MyAspNetCoreApp.Controllers
{
    public class ExamController : Controller
    {
        private readonly CareerGuideDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ExamController(CareerGuideDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Exam
        public async Task<IActionResult> Index()
        {
            var exams = await _context.Exams
                .Where(e => e.IsActive)
                .OrderByDescending(e => e.CreatedDate)
                .ToListAsync();
            
            return View(exams);
        }

        // GET: Exam/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams
                .Include(e => e.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }

        // GET: Exam/Take/5
        public async Task<IActionResult> Take(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams
                .Include(e => e.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(e => e.Id == id && e.IsActive);

            if (exam == null)
            {
                return NotFound();
            }

            // Shuffle questions and answers for randomization
            exam.Questions = exam.Questions.OrderBy(q => q.OrderNumber).ToList();
            foreach (var question in exam.Questions)
            {
                question.Answers = question.Answers.OrderBy(a => a.OrderNumber).ToList();
            }

            return View(exam);
        }

        // POST: Exam/Submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(int examId, Dictionary<int, int> answers)
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

            var exam = await _context.Exams
                .Include(e => e.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(e => e.Id == examId);

            if (exam == null)
            {
                return NotFound();
            }

            int correctAnswers = 0;
            int totalQuestions = exam.Questions.Count;

            // Save user answers and calculate score
            foreach (var question in exam.Questions)
            {
                if (answers.ContainsKey(question.Id))
                {
                    var selectedAnswerId = answers[question.Id];
                    var selectedAnswer = question.Answers.FirstOrDefault(a => a.Id == selectedAnswerId);
                    
                    if (selectedAnswer != null)
                    {
                        var userAnswer = new UserAnswer
                        {
                            UserId = user.Id,
                            QuestionId = question.Id,
                            AnswerId = selectedAnswerId,
                            AnsweredAt = DateTime.Now,
                            IsCorrect = selectedAnswer.IsCorrect
                        };

                        _context.UserAnswers.Add(userAnswer);

                        if (selectedAnswer.IsCorrect)
                        {
                            correctAnswers++;
                        }
                    }
                }
            }

            // Calculate score percentage
            int score = totalQuestions > 0 ? (correctAnswers * 100) / totalQuestions : 0;

            // Save exam result
            var examResult = new ExamResult
            {
                UserId = user.Id,
                ExamId = examId,
                Score = score,
                CorrectAnswers = correctAnswers,
                TotalQuestions = totalQuestions,
                TakenDate = DateTime.Now,
                TimeTaken = TimeSpan.FromMinutes(exam.Duration) // Assume full time taken
            };

            _context.ExamResults.Add(examResult);
            await _context.SaveChangesAsync();

            return RedirectToAction("Result", new { id = examResult.Id });
        }

        // GET: Exam/Result/5
        public async Task<IActionResult> Result(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _context.ExamResults
                .Include(er => er.Exam)
                .FirstOrDefaultAsync(er => er.Id == id);

            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        // GET: Exam/MyResults
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

            var results = await _context.ExamResults
                .Include(er => er.Exam)
                .Where(er => er.UserId == user.Id)
                .OrderByDescending(er => er.TakenDate)
                .ToListAsync();

            return View(results);
        }
    }
}
