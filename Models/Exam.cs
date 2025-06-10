using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAspNetCoreApp.Models
{
    public class Exam
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public int Duration { get; set; } // Duration in minutes

        public int TotalQuestions { get; set; }

        public bool IsActive { get; set; } = true;

        // Computed property for passing score (not stored in database)
        [NotMapped]
        public double PassingScore => 60.0; // Default passing score percentage

        // Navigation properties
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        public virtual ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();
    }
}
