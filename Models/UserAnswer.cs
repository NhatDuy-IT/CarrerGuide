using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAspNetCoreApp.Models
{
    public class UserAnswer
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty; // Identity User ID
        
        [Required]
        public int QuestionId { get; set; }
        
        public int? AnswerId { get; set; } // For multiple choice
        
        [StringLength(2000)]
        public string? AnswerText { get; set; } // For essay questions
        
        public DateTime AnsweredAt { get; set; }
        
        public bool IsCorrect { get; set; }
        
        // Navigation properties
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; } = null!;
        
        [ForeignKey("AnswerId")]
        public virtual Answer? Answer { get; set; }
    }
}
