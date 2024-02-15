using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace shedule_app.Models
{
    public class Tasks
    {
        [Key]
        [Required]
        
        int TaskId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        
        public DateOnly date { get; set; }
        [Required]
        public TimeOnly time { get; set; }
        [ForeignKey("IdUser")]
        public int IdUser { get; set; }
        public User Users { get; set; }
    }
}
