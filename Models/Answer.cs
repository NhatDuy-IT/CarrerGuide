using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAspNetCoreApp.Models
{
    public class Answer
    {
        public int Id { get; set; }
        
        [Required]
        public int QuestionId { get; set; }
        
        [Required]
        [StringLength(500)]
        public string AnswerText { get; set; } = string.Empty;

        // Alias for compatibility
        public string Text
        {
            get => AnswerText;
            set => AnswerText = value;
        }
        
        public bool IsCorrect { get; set; }
        
        public int OrderNumber { get; set; }
        
        // Navigation properties
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; } = null!;
        
        public virtual ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
    }
}
