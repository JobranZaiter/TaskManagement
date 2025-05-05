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
            var(status, message) = await projectService.AddProject(req);
            return StatusCode((int)status, message);
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProjectDetails(int projectId)
        {
            var(status, message, project) = await projectService.GetProjectDetails(projectId);
            return StatusCode((int)status, new { message, project });
        }
        
    }
}
