using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAspNetCoreApp.Models;

namespace MyAspNetCoreApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ExamController : Controller
    {
        private readonly CareerGuideDbContext _context;

        public ExamController(CareerGuideDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Exam
        public async Task<IActionResult> Index()
        {
            var exams = await _context.Exams
                .OrderByDescending(e => e.CreatedDate)
                .ToListAsync();
            return View(exams);
        }

        // GET: Admin/Exam/Create 
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Exam/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Duration,TotalQuestions")] Exam exam)
        {
            if (ModelState.IsValid)
            {
                exam.CreatedDate = DateTime.Now;
                exam.IsActive = true;
                _context.Add(exam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(exam);
        }

        // GET: Admin/Exam/Edit/5
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

        // POST: Admin/Exam/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Duration,TotalQuestions,IsActive")] Exam exam)
        {
            if (id != exam.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingExam = await _context.Exams.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
                    if (existingExam == null)
                    {
                        return NotFound();
                    }

                    exam.CreatedDate = existingExam.CreatedDate;
                    _context.Update(exam);
                    await _context.SaveChangesAsync();
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

        // GET: Admin/Exam/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }

        // POST: Admin/Exam/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam != null)
            {
                _context.Exams.Remove(exam);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ExamExists(int id)
        {
            return _context.Exams.Any(e => e.Id == id);
        }

        // GET: Admin/Exam/Questions/5
        public async Task<IActionResult> Questions(int? id)
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

        // GET: Admin/Exam/AddQuestion/5
        public async Task<IActionResult> AddQuestion(int? id)
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

            ViewBag.ExamId = id;
            return View(new Question { ExamId = exam.Id });
        }        // POST: Admin/Exam/AddQuestion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddQuestion([Bind("ExamId,QuestionText,Type,Points")] Question question)
        {
            if (ModelState.IsValid)
            {
                question.OrderNumber = await _context.Questions
                    .Where(q => q.ExamId == question.ExamId)
                    .CountAsync() + 1;

                _context.Questions.Add(question);
                await _context.SaveChangesAsync();

                // Process answers if they were submitted
                var answers = Request.Form["Answers[0].AnswerText"].Count;
                var correctAnswer = Request.Form["correctAnswer"].FirstOrDefault();

                for (int i = 0; i < answers; i++)
                {
                    var answerText = Request.Form[$"Answers[{i}].AnswerText"].ToString();
                    if (!string.IsNullOrEmpty(answerText))
                    {
                        var answer = new Answer
                        {
                            QuestionId = question.Id,
                            AnswerText = answerText,
                            IsCorrect = i.ToString() == correctAnswer
                        };
                        _context.Answers.Add(answer);
                    }
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Questions), new { id = question.ExamId });
            }
            return View(question);
        }
    }
}
