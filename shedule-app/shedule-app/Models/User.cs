using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace shedule_app.Models
{
    public class User   
    {
        [Key]
        [Required]
        public int IdUser { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserEmail { get; set; }
        [NotMapped]
        public string password { get; set; }
        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];

        public ICollection<Tasks> Tasks { get; set; }
    }
}
