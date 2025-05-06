using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Models.DTOs.Requests;
using TaskManagement.Services.Interface;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService projectService;
        public ProjectController(IProjectService _projectService)
        {
            this.projectService = _projectService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProject(ProjectReq req)
        {
            var (status, message) = await projectService.AddProject(req);
            return StatusCode((int)status, message);
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProjectDetails(int projectId)
        {
            var (status, message, project) = await projectService.GetProjectDetails(projectId);
            return StatusCode((int)status, new { message, project });
        }
        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProject(int projectId)
        {
            var (status, message) = await projectService.DeleteProject(projectId);
            return StatusCode((int)status, message);
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllProjects()
        {
            var (status, message, projects) = await projectService.GetAllProjects();
            return StatusCode((int)status, new { message, projects });
        }
        [HttpGet("assignments")]
        public async Task<IActionResult> GetProjectAssignments()
        {
            var (status, message, projects) = await projectService.GetProjectAssignments();
            return StatusCode((int)status, new { message, projects });
        }
<<<<<<< HEAD

=======
>>>>>>> c7e36ea0b1d9da653a031e74ce1a64a4bdc01a9f
        [HttpPost("{projectId}/assignee")]
        public async Task<IActionResult> AddAssigneeByEmail(int projectId, [FromBody] string assigneeEmail)
        {
            var (status, message) = await projectService.AddAssigneeByEmailAsync(projectId, assigneeEmail, "Member");
            return StatusCode((int)status, message);
        }
<<<<<<< HEAD

=======
>>>>>>> c7e36ea0b1d9da653a031e74ce1a64a4bdc01a9f
        [HttpPost("{projectId}/assignee/{role}")]
        public async Task<IActionResult> AddAssigneeByEmail(int projectId, string assigneeEmail, string role)
        {
            var (status, message) = await projectService.AddAssigneeByEmailAsync(projectId, assigneeEmail, role);
            return StatusCode((int)status, message);
        }
    }
}