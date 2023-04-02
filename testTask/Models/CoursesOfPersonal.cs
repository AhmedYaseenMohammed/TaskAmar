using System.ComponentModel.DataAnnotations;

namespace testTask.Models
{
    public class CoursesOfPersonal: BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int InformationId { get; set; }
        public Information Information { get; set; }
    }
}
