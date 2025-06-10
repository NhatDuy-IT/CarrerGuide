using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAspNetCoreApp.Models
{
    public class ExamResult
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty; // Changed to string for Identity

        [Required]
        public int ExamId { get; set; }

        [Range(0, 100)]
        public int Score { get; set; }

        public DateTime TakenDate { get; set; }

        public int CorrectAnswers { get; set; }

        public int TotalQuestions { get; set; }

        public TimeSpan TimeTaken { get; set; }

        // Navigation properties
        [ForeignKey("ExamId")]
        public virtual Exam Exam { get; set; } = null!;

        public int? SchoolId { get; set; }

        [ForeignKey("SchoolId")]
        public virtual School? School { get; set; }
    }
}
