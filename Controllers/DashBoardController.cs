using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Services.Implementation;
using TaskManagement.Services.Interface;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IProjectService projectService;
        private readonly ITaskService taskService;

        public DashboardController(IProjectService _projectService, TaskService _taskService)
        {
            projectService = _projectService;
            taskService = _taskService;
        }

        [HttpGet("owned-projects")]
        public async Task<IActionResult> GetOwnedProjects()
        {
            var (status, message, projects) = await projectService.GetAllProjects();
            return status == Models.Enum.ErrorType.Ok
                ? Ok(projects)
                : StatusCode((int)status, message);
        }

        [HttpGet("assigned-projects")]
        public async Task<IActionResult> GetProjectAssignments()
        {
            var (status, message, projects) = await projectService.GetProjectAssignments();
            return status == Models.Enum.ErrorType.Ok
                ? Ok(projects)
                : StatusCode((int)status, message);
        }

        [HttpGet("assigned-subtasks")]
        public async Task<IActionResult> GetAssignedSubTasks()
        {
            var (status, message, subtasks) = await taskService.GetAllSubTasks();
            return status == Models.Enum.ErrorType.Ok
                ? Ok(subtasks)
                : StatusCode((int)status, message);
        }
    }
}
