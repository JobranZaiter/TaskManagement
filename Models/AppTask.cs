using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Models
{
    public class AppTask
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        [Required]
        public int AssigneeId { get; set; }

        [ForeignKey("AssigneeId")]
        public ProjectAssignee Assignee{ get; set; }

        [Required]
        [StringLength(30)]
        public string Title { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public bool isCompleted { get; set; }=false;
        public List<Attachement> Attachements { get; set; } = new List<Attachement>();
        public List<TaskPermission> Permissions { get; set; } = new List<TaskPermission>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<SubTask> SubTasks { get; set; } = new List<SubTask>();
        public List<Log> Logs { get; set; } = new List<Log>();
    }
}
