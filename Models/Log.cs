using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Models
{
    public class Log
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TaskId { get; set; }

        [ForeignKey("TaskId")]
        public AppTask Task { get; set; }
        public int? SubTaskId { get; set; }

        [ForeignKey("SubTaskId")]
        public SubTask SubTask { get; set; }

        [Required]
        public int ProjectAssigneeId { get; set; }

        [ForeignKey("ProjectAssigneeId")]
        public ProjectAssignee ProjectAssignee { get; set; }

        [StringLength(100)]
        public string ActionType { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    }
}
