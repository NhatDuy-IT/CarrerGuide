using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAspNetCoreApp.Models
{
    public class SurveyQuestion
    {
        public int Id { get; set; }
        
        [Required]
        public int SurveyId { get; set; }
        
        [Required]
        [StringLength(500)]
        public string QuestionText { get; set; } = string.Empty;
        
        public int OrderNumber { get; set; }
        
        [StringLength(50)]
        public string Category { get; set; } = string.Empty; // E/I, S/N, T/F, J/P for MBTI
        
        // Navigation properties
        [ForeignKey("SurveyId")]
        public virtual Survey Survey { get; set; } = null!;
        
        public virtual ICollection<SurveyAnswer> Answers { get; set; } = new List<SurveyAnswer>();
    }
}
