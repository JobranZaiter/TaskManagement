using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User Owner { get; set; }

        [Required]
        [StringLength(30)]
        public string Title { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; }

        public List<ProjectAssignee> ProjectAssignees { get; set; } = new List<ProjectAssignee>();
        public List<AppTask> Tasks { get; set; } = new List<AppTask>();




    }
}
