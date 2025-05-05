using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Models
{
    public class SubTask
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TaskId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [RegularExpression("^(Awaiting|Pending|Finished)$", ErrorMessage = "Role must be Admin, User, or Manager.")]
        public string Status { get; set; } = "Awaiting";
        [ForeignKey("TaskId")]
        public AppTask Task { get; set; }

        [Required]
        public int ProjectAssigneeId { get; set; }

        [ForeignKey("ProjectAssigneeId")]
        public ProjectAssignee AssignedTo { get; set; }
        public List<Log> Logs { get; set; } = new List<Log>();
    }
}
