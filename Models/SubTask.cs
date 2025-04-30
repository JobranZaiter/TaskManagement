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

        [ForeignKey("TaskId")]
        public AppTask Task { get; set; }

        [Required]
        public int ProjectAssigneeId { get; set; }

        [ForeignKey("ProjectAssigneeId")]
        public ProjectAssignee AssignedTo { get; set; }
        public List<Log> Logs { get; set; } = new List<Log>();
    }
}
