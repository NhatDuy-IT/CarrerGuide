using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAspNetCoreApp.Models
{
    public class SurveyResult
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty; // Identity User ID
        
        [Required]
        public int SurveyId { get; set; }
        
        [StringLength(10)]
        public string Result { get; set; } = string.Empty; // MBTI result like "INTJ"
        
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        
        public DateTime CompletedDate { get; set; }
        
        public int TotalScore { get; set; }
        
        // Navigation properties
        [ForeignKey("SurveyId")]
        public virtual Survey Survey { get; set; } = null!;
    }
}
