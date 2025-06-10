using System.ComponentModel.DataAnnotations;

namespace MyAspNetCoreApp.Models
{
    public class Survey
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public bool IsActive { get; set; } = true;
        
        public SurveyType Type { get; set; } = SurveyType.MBTI;
        
        // Navigation properties
        public virtual ICollection<SurveyQuestion> Questions { get; set; } = new List<SurveyQuestion>();
        public virtual ICollection<SurveyResult> Results { get; set; } = new List<SurveyResult>();
    }
    
    public enum SurveyType
    {
        MBTI = 1,
        CareerAssessment = 2,
        PersonalityTest = 3
    }
}
