using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MyAspNetCoreApp.Models;

public class CareerGuideDbContext : IdentityDbContext<IdentityUser>
{
    public CareerGuideDbContext(DbContextOptions<CareerGuideDbContext> options) : base(options)
    {
    }

    // DbSets for all models
    public DbSet<Product> Products { get; set; }
    public DbSet<User> AppUsers { get; set; } // Renamed to avoid conflict with Identity User
    public DbSet<School> Schools { get; set; }
    public DbSet<Survey> Surveys { get; set; }
    public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
    public DbSet<SurveyAnswer> SurveyAnswers { get; set; }
    public DbSet<SurveyResult> SurveyResults { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<UserAnswer> UserAnswers { get; set; }
    public DbSet<ExamResult> ExamResults { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure decimal precision
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<School>()
            .Property(s => s.TuitionFee)
            .HasColumnType("decimal(18,2)");

        // Configure relationships
        modelBuilder.Entity<Question>()
            .HasOne(q => q.Exam)
            .WithMany(e => e.Questions)
            .HasForeignKey(q => q.ExamId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Answer>()
            .HasOne(a => a.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure UserAnswer relationships
        modelBuilder.Entity<UserAnswer>()
            .HasOne(ua => ua.Question)
            .WithMany(q => q.UserAnswers)
            .HasForeignKey(ua => ua.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserAnswer>()
            .HasOne(ua => ua.Answer)
            .WithMany(a => a.UserAnswers)
            .HasForeignKey(ua => ua.AnswerId)
            .OnDelete(DeleteBehavior.NoAction);

        // Configure UserAnswer with Identity User
        modelBuilder.Entity<UserAnswer>()
            .HasOne<IdentityUser>()
            .WithMany()
            .HasForeignKey(ua => ua.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure ExamResult relationships
        modelBuilder.Entity<ExamResult>()
            .HasOne(er => er.Exam)
            .WithMany(e => e.ExamResults)
            .HasForeignKey(er => er.ExamId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure ExamResult with Identity User
        modelBuilder.Entity<ExamResult>()
            .HasOne<IdentityUser>()
            .WithMany()
            .HasForeignKey(er => er.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SurveyQuestion>()
            .HasOne(sq => sq.Survey)
            .WithMany(s => s.Questions)
            .HasForeignKey(sq => sq.SurveyId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SurveyAnswer>()
            .HasOne(sa => sa.Question)
            .WithMany(sq => sq.Answers)
            .HasForeignKey(sa => sa.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SurveyResult>()
            .HasOne(sr => sr.Survey)
            .WithMany(s => s.Results)
            .HasForeignKey(sr => sr.SurveyId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed Data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Temporarily commented out to fix compilation issues
        // TODO: Fix seed data to match current model structure

        // Seed Schools
        /*modelBuilder.Entity<School>().HasData(
            new School
            {
                Id = 1,
                Name = "ĐẠI HỌC CÔNG NGHỆ TP.HCM (HUTECH)",
                Address = "475A Điện Biên Phủ, Phường 25, Quận Bình Thạnh, TP.HCM",
                PhoneNumber = "028-5445-7777",
                Description = "Trường đại học công nghệ hàng đầu tại TP.HCM với nhiều ngành học hot",
                Website = "https://www.hutech.edu.vn",
                ImageUrl = "/image/HUTECH2.jpg",
                TuitionFee = 25000000,
                EstablishedYear = new DateTime(1995, 1, 1)
            },
            new School
            {
                Id = 2,
                Name = "ĐẠI HỌC KHOA HỌC XÃ HỘI VÀ NHÂN VĂN",
                Address = "10-12 Đinh Tiên Hoàng, Quận 1, TP.HCM",
                PhoneNumber = "028-3829-2269",
                Description = "Trường đại học chuyên về khoa học xã hội và nhân văn",
                Website = "https://www.ussh.edu.vn",
                ImageUrl = "/image/nhanvan.png",
                TuitionFee = 20000000,
                EstablishedYear = new DateTime(1957, 1, 1)
            },
            new School
            {
                Id = 3,
                Name = "ĐẠI HỌC BÁCH KHOA TP.HCM",
                Address = "268 Lý Thường Kiệt, Quận 10, TP.HCM",
                PhoneNumber = "028-3865-4141",
                Description = "Trường đại học kỹ thuật hàng đầu Việt Nam",
                Website = "https://www.hcmut.edu.vn",
                ImageUrl = "/image/bachkhoa.jpg",
                TuitionFee = 30000000,
                EstablishedYear = new DateTime(1957, 1, 1)
            }
        );*/

        // Seed Products (Schools as products)
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "ĐẠI HỌC CÔNG NGHỆ TP.HCM",
                ImageUrl = "/image/HUTECH2.jpg",
                Price = 25000000,
                IsFeatured = true,
                IsNew = false,
                Description = "Trường đại học công nghệ hàng đầu với nhiều ngành học hot",
                CreatedDate = new DateTime(2024, 1, 1)
            },
            new Product
            {
                Id = 2,
                Name = "ĐẠI HỌC XÃ HỘI VÀ NHÂN VĂN",
                ImageUrl = "/image/nhanvan.png",
                Price = 20000000,
                IsFeatured = true,
                IsNew = true,
                Description = "Trường đại học chuyên về khoa học xã hội và nhân văn",
                CreatedDate = new DateTime(2024, 1, 1)
            },
            new Product
            {
                Id = 3,
                Name = "ĐẠI HỌC BÁCH KHOA TP.HCM",
                ImageUrl = "/image/bachkhoa.jpg",
                Price = 30000000,
                IsFeatured = false,
                IsNew = true,
                Description = "Trường đại học kỹ thuật hàng đầu Việt Nam",
                CreatedDate = new DateTime(2024, 1, 1)
            }
        );

        // Seed Surveys
        modelBuilder.Entity<Survey>().HasData(
            new Survey
            {
                Id = 1,
                Title = "MBTI Personality Assessment",
                Description = "Discover your personality type with the Myers-Briggs Type Indicator",
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true,
                Type = SurveyType.MBTI
            },
            new Survey
            {
                Id = 2,
                Title = "Career Interest Survey",
                Description = "Find careers that match your interests and skills",
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true,
                Type = SurveyType.CareerAssessment
            }
        );

        // Seed Exams
        modelBuilder.Entity<Exam>().HasData(
            new Exam
            {
                Id = 1,
                Title = "Đề thi Toán học",
                Description = "Bài kiểm tra kiến thức toán học cơ bản",
                CreatedDate = new DateTime(2024, 6, 1),
                Duration = 90,
                TotalQuestions = 20,
                IsActive = true
            },
            new Exam
            {
                Id = 2,
                Title = "Đề thi Tiếng Anh",
                Description = "Bài kiểm tra năng lực tiếng Anh",
                CreatedDate = new DateTime(2024, 6, 2),
                Duration = 60,
                TotalQuestions = 30,
                IsActive = true
            },
            new Exam
            {
                Id = 3,
                Title = "Đề thi Vật lý",
                Description = "Bài kiểm tra kiến thức vật lý",
                CreatedDate = new DateTime(2024, 6, 3),
                Duration = 90,
                TotalQuestions = 25,
                IsActive = true
            }
        );

        // Seed Questions for Math Exam
        modelBuilder.Entity<Question>().HasData(
            new Question
            {
                Id = 1,
                ExamId = 1,
                QuestionText = "Tính đạo hàm của hàm số f(x) = x² + 3x - 2",
                Type = QuestionType.MultipleChoice,
                Points = 5,
                OrderNumber = 1
            },
            new Question
            {
                Id = 2,
                ExamId = 1,
                QuestionText = "Giải phương trình: 2x + 5 = 13",
                Type = QuestionType.MultipleChoice,
                Points = 3,
                OrderNumber = 2
            },
            new Question
            {
                Id = 3,
                ExamId = 1,
                QuestionText = "Tính tích phân ∫(2x + 1)dx từ 0 đến 2",
                Type = QuestionType.MultipleChoice,
                Points = 7,
                OrderNumber = 3
            },
            // English Exam Questions
            new Question
            {
                Id = 4,
                ExamId = 2,
                QuestionText = "Choose the correct form: 'I _____ to school every day.'",
                Type = QuestionType.MultipleChoice,
                Points = 2,
                OrderNumber = 1
            },
            new Question
            {
                Id = 5,
                ExamId = 2,
                QuestionText = "What is the past tense of 'go'?",
                Type = QuestionType.MultipleChoice,
                Points = 2,
                OrderNumber = 2
            },
            // Physics Exam Questions
            new Question
            {
                Id = 6,
                ExamId = 3,
                QuestionText = "Công thức tính vận tốc là gì?",
                Type = QuestionType.MultipleChoice,
                Points = 4,
                OrderNumber = 1
            }
        );

        // Seed Answers
        modelBuilder.Entity<Answer>().HasData(
            // Answers for Question 1 (Math - Derivative)
            new Answer { Id = 1, QuestionId = 1, AnswerText = "f'(x) = 2x + 3", IsCorrect = true, OrderNumber = 1 },
            new Answer { Id = 2, QuestionId = 1, AnswerText = "f'(x) = x² + 3", IsCorrect = false, OrderNumber = 2 },
            new Answer { Id = 3, QuestionId = 1, AnswerText = "f'(x) = 2x - 2", IsCorrect = false, OrderNumber = 3 },
            new Answer { Id = 4, QuestionId = 1, AnswerText = "f'(x) = 2x + 5", IsCorrect = false, OrderNumber = 4 },

            // Answers for Question 2 (Math - Equation)
            new Answer { Id = 5, QuestionId = 2, AnswerText = "x = 4", IsCorrect = true, OrderNumber = 1 },
            new Answer { Id = 6, QuestionId = 2, AnswerText = "x = 3", IsCorrect = false, OrderNumber = 2 },
            new Answer { Id = 7, QuestionId = 2, AnswerText = "x = 5", IsCorrect = false, OrderNumber = 3 },
            new Answer { Id = 8, QuestionId = 2, AnswerText = "x = 6", IsCorrect = false, OrderNumber = 4 },

            // Answers for Question 3 (Math - Integral)
            new Answer { Id = 9, QuestionId = 3, AnswerText = "6", IsCorrect = true, OrderNumber = 1 },
            new Answer { Id = 10, QuestionId = 3, AnswerText = "4", IsCorrect = false, OrderNumber = 2 },
            new Answer { Id = 11, QuestionId = 3, AnswerText = "8", IsCorrect = false, OrderNumber = 3 },
            new Answer { Id = 12, QuestionId = 3, AnswerText = "5", IsCorrect = false, OrderNumber = 4 },

            // Answers for Question 4 (English)
            new Answer { Id = 13, QuestionId = 4, AnswerText = "go", IsCorrect = true, OrderNumber = 1 },
            new Answer { Id = 14, QuestionId = 4, AnswerText = "goes", IsCorrect = false, OrderNumber = 2 },
            new Answer { Id = 15, QuestionId = 4, AnswerText = "going", IsCorrect = false, OrderNumber = 3 },
            new Answer { Id = 16, QuestionId = 4, AnswerText = "went", IsCorrect = false, OrderNumber = 4 },

            // Answers for Question 5 (English)
            new Answer { Id = 17, QuestionId = 5, AnswerText = "went", IsCorrect = true, OrderNumber = 1 },
            new Answer { Id = 18, QuestionId = 5, AnswerText = "go", IsCorrect = false, OrderNumber = 2 },
            new Answer { Id = 19, QuestionId = 5, AnswerText = "goes", IsCorrect = false, OrderNumber = 3 },
            new Answer { Id = 20, QuestionId = 5, AnswerText = "going", IsCorrect = false, OrderNumber = 4 },

            // Answers for Question 6 (Physics)
            new Answer { Id = 21, QuestionId = 6, AnswerText = "v = s/t", IsCorrect = true, OrderNumber = 1 },
            new Answer { Id = 22, QuestionId = 6, AnswerText = "v = s*t", IsCorrect = false, OrderNumber = 2 },
            new Answer { Id = 23, QuestionId = 6, AnswerText = "v = t/s", IsCorrect = false, OrderNumber = 3 },
            new Answer { Id = 24, QuestionId = 6, AnswerText = "v = s + t", IsCorrect = false, OrderNumber = 4 }
        );

        // Seed MBTI Survey Questions
        modelBuilder.Entity<SurveyQuestion>().HasData(
            new SurveyQuestion
            {
                Id = 1,
                SurveyId = 1,
                QuestionText = "Bạn thích giao tiếp với nhiều người hay chỉ với một vài người thân thiết?",
                OrderNumber = 1,
                Category = "E/I"
            },
            new SurveyQuestion
            {
                Id = 2,
                SurveyId = 1,
                QuestionText = "Bạn thích tập trung vào thực tế hay vào khả năng và ý tưởng?",
                OrderNumber = 2,
                Category = "S/N"
            },
            new SurveyQuestion
            {
                Id = 3,
                SurveyId = 1,
                QuestionText = "Khi đưa ra quyết định, bạn dựa vào logic hay cảm xúc?",
                OrderNumber = 3,
                Category = "T/F"
            },
            new SurveyQuestion
            {
                Id = 4,
                SurveyId = 1,
                QuestionText = "Bạn thích có kế hoạch rõ ràng hay linh hoạt thay đổi?",
                OrderNumber = 4,
                Category = "J/P"
            },
            new SurveyQuestion
            {
                Id = 5,
                SurveyId = 1,
                QuestionText = "Trong một buổi tiệc, bạn thường:",
                OrderNumber = 5,
                Category = "E/I"
            }
        );

        // Seed MBTI Survey Answers
        modelBuilder.Entity<SurveyAnswer>().HasData(
            // Question 1 answers (E/I)
            new SurveyAnswer { Id = 1, QuestionId = 1, AnswerText = "Giao tiếp với nhiều người", Points = 1, PersonalityType = "E", OrderNumber = 1 },
            new SurveyAnswer { Id = 2, QuestionId = 1, AnswerText = "Chỉ với một vài người thân thiết", Points = 1, PersonalityType = "I", OrderNumber = 2 },

            // Question 2 answers (S/N)
            new SurveyAnswer { Id = 3, QuestionId = 2, AnswerText = "Tập trung vào thực tế", Points = 1, PersonalityType = "S", OrderNumber = 1 },
            new SurveyAnswer { Id = 4, QuestionId = 2, AnswerText = "Tập trung vào khả năng và ý tưởng", Points = 1, PersonalityType = "N", OrderNumber = 2 },

            // Question 3 answers (T/F)
            new SurveyAnswer { Id = 5, QuestionId = 3, AnswerText = "Dựa vào logic", Points = 1, PersonalityType = "T", OrderNumber = 1 },
            new SurveyAnswer { Id = 6, QuestionId = 3, AnswerText = "Dựa vào cảm xúc", Points = 1, PersonalityType = "F", OrderNumber = 2 },

            // Question 4 answers (J/P)
            new SurveyAnswer { Id = 7, QuestionId = 4, AnswerText = "Có kế hoạch rõ ràng", Points = 1, PersonalityType = "J", OrderNumber = 1 },
            new SurveyAnswer { Id = 8, QuestionId = 4, AnswerText = "Linh hoạt thay đổi", Points = 1, PersonalityType = "P", OrderNumber = 2 },

            // Question 5 answers (E/I)
            new SurveyAnswer { Id = 9, QuestionId = 5, AnswerText = "Nói chuyện với nhiều người", Points = 1, PersonalityType = "E", OrderNumber = 1 },
            new SurveyAnswer { Id = 10, QuestionId = 5, AnswerText = "Tìm góc yên tĩnh để thư giãn", Points = 1, PersonalityType = "I", OrderNumber = 2 }
        );
    }
}