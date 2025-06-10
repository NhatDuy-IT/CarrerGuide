namespace MyAspNetCoreApp.Models
{
    public class SchoolDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty; // Mapped from Address
        public string Website { get; set; } = string.Empty;
        public decimal? TuitionFee { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int EstablishedYear { get; set; }
        public string Type { get; set; } = string.Empty; // Computed based on TuitionFee
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Static method to convert from School entity
        public static SchoolDto FromSchool(School school)
        {
            return new SchoolDto
            {
                Id = school.Id,
                Name = school.Name,
                Description = school.Description,
                Location = school.Address, // Use Address as Location
                Website = school.Website,
                TuitionFee = school.TuitionFee,
                ImageUrl = school.ImageUrl,
                EstablishedYear = school.EstablishedYear, // Already int
                Type = !string.IsNullOrEmpty(school.Type) ? school.Type :
                       (school.TuitionFee > 20000000 ? "Tư thục" : "Công lập"), // Use school.Type if available
                CreatedDate = DateTime.Now
            };
        }

        // Static method to convert list
        public static List<SchoolDto> FromSchoolList(List<School> schools)
        {
            return schools.Select(FromSchool).ToList();
        }
    }
}
