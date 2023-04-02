using System.ComponentModel.DataAnnotations;

using testTask.Healper;

namespace testTask.Models
{
    public class Users: BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        public string? Password { get; set; }
        public Role role { get; set; }
    }
}
