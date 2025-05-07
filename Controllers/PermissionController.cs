using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Models.DTOs.Requests;
using TaskManagement.Services.Interface;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService permissionService;

        public PermissionController(IPermissionService _permissionService)
        {
            permissionService = _permissionService;
        }
        [HttpGet("project/{projectId}/permission")]
        public async Task<IActionResult> GetPermissions(int projectId)
        {
            var (status, message, permissions) = await permissionService.GetAllPermissionsForProject(projectId);
            return StatusCode((int)status, new { message, permissions });
        }
        [HttpPost("project/{projectId}/permission")]
        public async Task<IActionResult> AddPermission(int projectId, [FromBody] PermissionCreateReq request)
        {
            var (status, message) = await permissionService.AddPermission(request, projectId);
            return StatusCode((int)status, new { message });
        }

        [HttpDelete("project/{projectId}/permission/{permissionId}/{taskId}")]
        public async Task<IActionResult> DeletePermission(int projectId, int taskId, int permissionId)
        {
            var (status, message) = await permissionService.DeletePermission(taskId, permissionId, projectId);
            return StatusCode((int)status, new { message });
        }
    }
}
