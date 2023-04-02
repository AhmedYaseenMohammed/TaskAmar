namespace testTask.Models
{
    public class BaseEntity
    {
        public DateTime InsertDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
