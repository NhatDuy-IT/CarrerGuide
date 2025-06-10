using System.ComponentModel.DataAnnotations;

namespace MyAspNetCoreApp.Models
{
    public class School
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [StringLength(200)]
        public string Website { get; set; } = string.Empty;

        [StringLength(200)]
        public string ImageUrl { get; set; } = string.Empty;

        public decimal TuitionFee { get; set; }

        [Range(1800, 2030, ErrorMessage = "Năm thành lập phải từ 1800 đến 2030")]
        public int EstablishedYear { get; set; }

        [StringLength(50)]
        public string Type { get; set; } = string.Empty; // Loại trường: Công lập, Tư thục, Dân lập

        // [StringLength(100)]
        // public string Location { get; set; } = string.Empty; // Vị trí: Hà Nội, TP.HCM, etc.

        // public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();
    }
}
