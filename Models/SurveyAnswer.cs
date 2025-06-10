using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAspNetCoreApp.Models
{
    public class SurveyAnswer
    {
        public int Id { get; set; }
        
        [Required]
        public int QuestionId { get; set; }
        
        [Required]
        [StringLength(200)]
        public string AnswerText { get; set; } = string.Empty;
        
        public int Points { get; set; } = 1;
        
        [StringLength(10)]
        public string PersonalityType { get; set; } = string.Empty; // E, I, S, N, T, F, J, P
        
        public int OrderNumber { get; set; }
        
        // Navigation properties
        [ForeignKey("QuestionId")]
        public virtual SurveyQuestion Question { get; set; } = null!;
    }
}
