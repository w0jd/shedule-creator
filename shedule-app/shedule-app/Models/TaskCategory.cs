using System.ComponentModel.DataAnnotations.Schema;

namespace shedule_app.Models
{
    public class TaskCategory
    {
        [ForeignKey("IdCategory")]
        public int IdCategory { get; set; }
        public Category Categories { get; set; }
        [ForeignKey("TaskId")]
        public int TaskId { get; set; }
        public Tasks Tasks { get; set; }
    }
}
