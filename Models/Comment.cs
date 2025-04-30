using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Models
{
    public class Comment
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
        public ProjectAssignee ProjectAssignee { get; set; }

        [Required]
        [StringLength(150)]
        public string Text { get; set; }
    }
}
