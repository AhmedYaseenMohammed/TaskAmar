using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace testTask.Models
{
    public class Information: BaseEntity
    {
        [Key]
        public int InfoId { get; set; }
        [Required]
        public string StatisticalNumber { get; set; }
        public string MilitaryRank { get; set; }
        [Required]
        public string FullName { get; set; }
        public string Brith { get; set; }
        [Required]
        public string CourseNumber { get; set; }
        public DateTime CourseDate { get; set; }
        public DateTime JoiningDate { get; set; }
        public string AcademicAchievement { get; set; }
        public string Notes { get; set; }
        [JsonIgnore]
        public List<PromotionsOfPersonal> promotionsOfPersonals { get; set; }
        [JsonIgnore]
        public List<CoursesOfPersonal> coursesOfPersonals { get;set; }
    }
}
