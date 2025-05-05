using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models.DTOs.Requests
{
    public class ProjectReq
    {
        [Required]
        [StringLength(30)]
        public string Title { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; }

    }
}
