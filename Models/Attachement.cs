using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Models
{
    public class Attachement
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
        public ProjectAssignee Poster { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(150)]
        public string? Description { get; set; }

        [Required]
        public string FilePath { get; set; }


    }
}
