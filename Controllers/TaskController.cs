using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Models.DTOs.Requests;
using TaskManagement.Services.Interface;

namespace TaskManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService taskService;
        public TaskController(ITaskService _taskService)
        {
            this.taskService = _taskService;
        }

        [HttpDelete("project/{projectId}/task/{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var (code, message) = await taskService.DeleteTask(taskId);
            return StatusCode((int)code, new { message });
        }
        [HttpPost("project/{projectId}/task")]
        public async Task<IActionResult> AddTask(int projectId, TaskCreateReq request)
        {
            var (code, message) = await taskService.AddTaskToProjectAsync(projectId, request);
            return StatusCode((int)code, new { message });
        }

        [HttpGet("project/{projectId}/{taskId}")]
        public async Task<IActionResult> GetAllSubTasks(int taskId)
        {
            var (status, message, tasks) = await taskService.GetSubTasks(taskId);
            return StatusCode((int)status, new { message, tasks });
        }


        [HttpPost("project/{projectId}/{taskId}")]
        public async Task<IActionResult> AddSubtask(int taskId, SubTaskReq request)
        {
            var (status, message) = await taskService.AddSubTaskToTaskAsync(taskId, request);
            return StatusCode((int)status, new { message});
        }

        [HttpDelete("project/{projectId}/{taskId}/{subtaskId}")]
        public async Task<IActionResult> DeleteSubTask(int taskId, int subtaskId)
        {
            var (status, message) = await taskService.DeleteSubTaskFromTaskAsync(taskId, subtaskId);
            return StatusCode((int)status, new { message });
        }

        [HttpPut("project/{projectId}/{taskId}/{subtaskId}")]
        public async Task<IActionResult> UpdateSubTaskStatus(int taskId, int subtaskId, [FromQuery] string status)
        {
            var (code, message) = await taskService.UpdateSubTask(taskId, subtaskId, status);
            return StatusCode((int)code, new { message });
        }


    }
}
