using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAspNetCoreApp.Models;

namespace MyAspNetCoreApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ExamManagementController : Controller
    {
        private readonly CareerGuideDbContext _context;

        public ExamManagementController(CareerGuideDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ExamManagement
        public async Task<IActionResult> Index()
        {
            var exams = await _context.Exams
                .Include(e => e.Questions)
                .ToListAsync();
            return View(exams);
        }

        // GET: Admin/ExamManagement/Details/5
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

        // GET: Admin/ExamManagement/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ExamManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Duration,PassingScore")] Exam exam)
        {
            if (ModelState.IsValid)
            {
                exam.CreatedDate = DateTime.Now;
                _context.Add(exam);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Bài thi đã được tạo thành công.";
                return RedirectToAction(nameof(Details), new { id = exam.Id });
            }
            return View(exam);
        }

        // GET: Admin/ExamManagement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams.FindAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
            return View(exam);
        }

        // POST: Admin/ExamManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Duration,PassingScore,CreatedDate")] Exam exam)
        {
            if (id != exam.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exam);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Bài thi đã được cập nhật.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExamExists(exam.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(exam);
        }

        // GET: Admin/ExamManagement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams
                .Include(e => e.Questions)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }

        // POST: Admin/ExamManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam != null)
            {
                _context.Exams.Remove(exam);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Bài thi đã được xóa.";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/ExamManagement/ManageQuestions/5
        public async Task<IActionResult> ManageQuestions(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams
                .Include(e => e.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }

        // GET: Admin/ExamManagement/AddQuestion/5
        public async Task<IActionResult> AddQuestion(int? examId)
        {
            if (examId == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams.FindAsync(examId);
            if (exam == null)
            {
                return NotFound();
            }

            ViewBag.ExamId = examId;
            ViewBag.ExamTitle = exam.Title;
            return View();
        }

        // POST: Admin/ExamManagement/AddQuestion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddQuestion([Bind("ExamId,Text,Type")] Question question, List<string> answerTexts, List<bool> isCorrect)
        {
            if (ModelState.IsValid && answerTexts.Count >= 2)
            {
                _context.Add(question);
                await _context.SaveChangesAsync();

                // Add answers
                for (int i = 0; i < answerTexts.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(answerTexts[i]))
                    {
                        var answer = new Answer
                        {
                            QuestionId = question.Id,
                            Text = answerTexts[i],
                            IsCorrect = i < isCorrect.Count && isCorrect[i]
                        };
                        _context.Add(answer);
                    }
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = "Câu hỏi đã được thêm thành công.";
                return RedirectToAction(nameof(ManageQuestions), new { id = question.ExamId });
            }

            ViewBag.ExamId = question.ExamId;
            var exam = await _context.Exams.FindAsync(question.ExamId);
            ViewBag.ExamTitle = exam?.Title;
            return View(question);
        }

        private bool ExamExists(int id)
        {
            return _context.Exams.Any(e => e.Id == id);
        }
    }
}
