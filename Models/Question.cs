using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAspNetCoreApp.Models
{
    public class Question
    {
        public int Id { get; set; }
        
        [Required]
        public int ExamId { get; set; }
        
        [Required]
        [StringLength(1000)]
        public string QuestionText { get; set; } = string.Empty;
        
        public QuestionType Type { get; set; } = QuestionType.MultipleChoice;
        
        public int Points { get; set; } = 1;
        
        public int OrderNumber { get; set; }
        
        // Navigation properties
        [ForeignKey("ExamId")]
        public virtual Exam Exam { get; set; } = null!;
        
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
        public virtual ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
    }
    
    public enum QuestionType
    {
        MultipleChoice = 1,
        TrueFalse = 2,
        Essay = 3
    }
}
