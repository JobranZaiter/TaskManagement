using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Models
{
    public class ProjectAssignee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        [RegularExpression("^(Owner|Admin|Manager|Employee)$", ErrorMessage = "Role must be Admin, User, or Manager.")]
        public string Role { get; set; }
        // App tasks they created
        public List<AppTask> AppTasks { get; set; }

        // Subtasks they are assigned to
        public List<SubTask> SubTasks { get; set; }
        public List<TaskPermission> Permissions { get; set; } = new List<TaskPermission>();
        public List<Attachement> Attachements { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
        public List<Log> Logs { get; set; } = new();

    }
}
