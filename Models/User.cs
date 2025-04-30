using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }
         
        [Required]
        public string PasswordSalt { get; set; }

        public List<Project> ProjectOwnerships { get; set; } = new List<Project>();
        
        public List<ProjectAssignee> ProjectAssignments { get; set; }


    }
}
