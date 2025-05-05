using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskManagement.Models;
using TaskManagement.Models.DTOs.Requests;
using TaskManagement.Models.DTOs.Responses;
using TaskManagement.Models.Enum;
using TaskManagement.Repository.Implementation;
using TaskManagement.Repository.Interface;
using TaskManagement.Services.Interface;

namespace TaskManagement.Services.Implementation
{
    public class TaskService : ITaskService
    {
        private readonly IAppTaskRepository taskRepository;
        private readonly ISubTaskRepository subTaskRepository;
        private readonly IUserRepository userRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IProjectRepository projectRepository;
        private readonly IPermissionManager permissionManager;
        private readonly IProjectAssigneeRepository projectAssigneeRepository;
        public TaskService(IProjectAssigneeRepository _projectAssigneeRepository, IPermissionManager _permissionManager, IProjectRepository _projectRepository, IHttpContextAccessor _httpContextAccessor, ISubTaskRepository _subTaskRepository, IAppTaskRepository _appTaskRepository, IUserRepository _userRepository ) 
        {
            this.projectAssigneeRepository = _projectAssigneeRepository;
            this.projectRepository = _projectRepository;
            this.permissionManager = _permissionManager;
            this.httpContextAccessor = _httpContextAccessor;
            this.subTaskRepository = _subTaskRepository;
            this.taskRepository = _appTaskRepository;
            this.userRepository = _userRepository;
        }
        private int? GetCurrentUserId()
        {
            var id = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(id, out var userId) ? userId : null;
        }
        public async Task<(ErrorType HttpCode, string Message, List<AssignedSubTaskResp>? Tasks)> GetAllSubTasks()
        {
            var id = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null)
                return (ErrorType.Unauthorized, "Please login first.", null);
            int userId = int.Parse(id);
            if (!await userRepository.UserExistsAsync(userId))
                return (ErrorType.Unauthorized, "User does not exist", null);

            var subTasks = await subTaskRepository.GetByUserIdAsync(userId);
            if (subTasks == null || subTasks.Count == 0)
                return (ErrorType.NotFound, "No tasks found", null);
            List<AssignedSubTaskResp> assignedSubTasks = subTasks.Select(s => new AssignedSubTaskResp
            {
                Id = s.Id,
                Description = s.Description,
                TaskId = s.TaskId,
                Status = s.Status
            }).ToList(); ;

            return (ErrorType.Ok, "Tasks found", assignedSubTasks);
        }

        public async Task<(ErrorType HttpCode, string Message, List<AppTask>? Tasks)> GetAllProjectTasks(int projectId)
        {
            var id = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null)
                return (ErrorType.Unauthorized, "Please login first.", null);
            int userId = int.Parse(id);
            if (!await userRepository.UserExistsAsync(userId))
                return (ErrorType.Unauthorized, "User does not exist", null);
            var tasks = await taskRepository.GetByUserIdAsync(userId);
            
            if (tasks == null || tasks.Count == 0)
                return (ErrorType.NotFound, "No tasks found", null);
            List<AppTask> permittedView = new List<AppTask>();
            foreach (var task in tasks)
            {
                var taskPermissions = await permissionManager.UserHasPermissionAsync(userId, task.Id, PermissionType.Read);
                if (taskPermissions)
                {
                    permittedView.Add(task);
                }
            }
            if (permittedView == null || permittedView.Count == 0)
                return (ErrorType.NotFound, "No tasks found", null);

            return (ErrorType.Ok, "Tasks found", tasks);
        }

        public async Task<(ErrorType HttpCode, string Message)> AddTaskToProjectAsync(int projectId, TaskCreateReq req)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return (ErrorType.Unauthorized, "Please login first");

            User user = await userRepository.GetByIdAsync(userId.Value);
            if (user == null)
                return (ErrorType.Unauthorized, "User does not exist");

            var project = await projectRepository.GetByIdAsync(projectId);
            if (project == null)
                return (ErrorType.NotFound, "Project not found");

            var assignee = await projectAssigneeRepository.GetByProjectAndUserIdAsync(projectId, userId.Value);
            if (assignee.Role != "Admin" || assignee.Role != "Manager" || project.UserId != userId )
                return (ErrorType.Unauthorized, "You do not have permission to add tasks to this project");

            var task = new AppTask
            {
                Title = req.Title,
                Description = req.Description,
                DueDate = req.DueDate,
                ProjectId = projectId,
                AssigneeId = assignee.Id,
                isCompleted = false
            };

            project.Tasks.Add(task);
            await projectRepository.SaveChangesAsync();

            return (ErrorType.Ok, "Task added successfully");
        }

    }
}
