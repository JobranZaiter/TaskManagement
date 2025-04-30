using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManagement.Models.Enum;

namespace TaskManagement.Models
{
    public class TaskPermission
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProjectAssigneeId { get; set; }

        [ForeignKey("ProjectAssigneeId")]
        public ProjectAssignee ProjectAssignee { get; set; }

        [Required]
        public int TaskId { get; set; }

        [ForeignKey("TaskId")]
        public AppTask Task { get; set; }

        [Required]
        public PermissionType Permission { get; set; }


    }
}
