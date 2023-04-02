using System.ComponentModel.DataAnnotations;

namespace testTask.Models
{
    public class PromotionsOfPersonal: BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public int InformationId { get; set; }
        public Information Information { get; set; }
    }
}
