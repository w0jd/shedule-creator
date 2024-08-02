using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
namespace shedule_app.Models
{
    public class Category 
    {
        [Required]
        [Key]
        public int IdCategory { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public ICollection<Tasks> Tasks { get; set; }
    }
}
