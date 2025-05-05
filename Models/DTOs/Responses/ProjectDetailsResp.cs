using System.Collections.Generic;

namespace TaskManagement.Models.DTOs.Responses
{
    public class ProjectDetailsResp
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public List<ProjectAssigneeInfo> AssignedUsers { get; set; } = new();
        public List<TaskInfo> Tasks { get; set; } = new();
    }

    public class ProjectAssigneeInfo
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class TaskInfo
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
