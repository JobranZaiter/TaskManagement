using System.Security.Claims;
using TaskManagement.Models;
using TaskManagement.Models.DTOs.Requests;
using TaskManagement.Models.Enum;
using TaskManagement.Repository.Interface;
using TaskManagement.Services.Interface;
using System.Text.Json.Serialization;
using TaskManagement.Models.DTOs.Responses;

namespace TaskManagement.Services.Implementation
{
    public class PermissionService : IPermissionService
    {
        private readonly IUserRepository userRepository;
        private readonly IProjectRepository projectRepository;
        private readonly IAppTaskRepository taskRepository;
        private readonly ISubTaskRepository subTaskRepository;
        private readonly IProjectAssigneeRepository projectAssigneeRepository;
        private readonly ITaskPermissionRepository taskPermissionRepository;
        private readonly IPermissionManager permissionManager;
        private readonly IHttpContextAccessor contextAccessor;

        public PermissionService(
            IPermissionManager _permissionManager,
            IUserRepository _userRepository,
            IProjectRepository _projectRepository,
            IAppTaskRepository _taskRepository,
            ISubTaskRepository _subTaskRepository,
            IProjectAssigneeRepository _projectAssignee,
            ITaskPermissionRepository _taskPermissionRepository,
            IHttpContextAccessor _contextAccessor
        )
        {
            this.contextAccessor = _contextAccessor;
            this.permissionManager = _permissionManager;
            this.taskPermissionRepository = _taskPermissionRepository;
            this.userRepository = _userRepository;
            this.projectRepository = _projectRepository;
            this.taskRepository = _taskRepository;
            this.subTaskRepository = _subTaskRepository;
            this.projectAssigneeRepository = _projectAssignee;
        }

        private int? GetCurrentUserId()
        {
            var id = contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(id, out var userId) ? userId : null;
        }

        public async Task<(ErrorType HttpCode, string Message)> AddPermission(PermissionCreateReq request, int projectId)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return (ErrorType.Unauthorized, "Please login first.");

            var user = await userRepository.GetByIdAsync(userId.Value);
            if (user == null)
                return (ErrorType.Unauthorized, "User does not exist");

            var project = await projectRepository.GetByIdAsync(projectId);
            if (project == null)
                return (ErrorType.NotFound, "Project not found");

            ProjectAssignee assignee = await projectAssigneeRepository.GetByProjectAndUserIdAsync(projectId, userId.Value);
            if (assignee == null && userId != project.UserId)
                return (ErrorType.Unauthorized, "You are not authorized to add permissions to this task");

            if (assignee?.Role != "Admin" && userId != project.UserId)
                return (ErrorType.Unauthorized, "Only project owner or admin can assign permissions");

            var task = await taskRepository.GetByIdAsync(request.TaskId);
            if (task == null)
                return (ErrorType.NotFound, "Task not found");

            var targetAssignee = await projectAssigneeRepository.GetByIdAsync(request.AssigneeId);
            if (targetAssignee == null)
                return (ErrorType.NotFound, "Target project assignee not found");

            if (targetAssignee.ProjectId != projectId)
                return (ErrorType.BadRequest, "Assignee does not belong to the same project");

            var existingPermissions = await taskPermissionRepository.GetByUserAndTaskIdsAsync(targetAssignee.UserId, request.TaskId);
            if (existingPermissions.Any(p => p.Permission == request.PermissionValue))
                return (ErrorType.BadRequest, "Permission already exists for this assignee on the task");

            var newPermission = new TaskPermission
            {
                ProjectAssigneeId = targetAssignee.Id,
                TaskId = request.TaskId,
                Permission = request.PermissionValue
            };

            await taskPermissionRepository.AddAsync(newPermission);
            await taskPermissionRepository.SaveChangesAsync();

            return (ErrorType.Ok, "Permission added successfully");
        }
        public async Task<(ErrorType HttpCode, string Message)> DeletePermission(int taskId, int permissionId, int projectId)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return (ErrorType.Unauthorized, "Please login first.");

            var user = await userRepository.GetByIdAsync(userId.Value);
            if (user == null)
                return (ErrorType.Unauthorized, "User does not exist");

            var project = await projectRepository.GetByIdAsync(projectId);
            if (project == null)
                return (ErrorType.NotFound, "Project not found");

            var assignee = await projectAssigneeRepository.GetByProjectAndUserIdAsync(projectId, userId.Value);
            if (assignee == null && userId != project.UserId)
                return (ErrorType.Unauthorized, "You are not authorized to delete permissions from this task");

            if (assignee?.Role != "Admin" && userId != project.UserId)
                return (ErrorType.Unauthorized, "Only project owner or admin can remove permissions");

            var task = await taskRepository.GetByIdAsync(taskId);
            if (task == null)
                return (ErrorType.NotFound, "Task not found");

            var permission = await taskPermissionRepository.GetByIdAsync(permissionId);
            if (permission == null || permission.TaskId != taskId)
                return (ErrorType.NotFound, "Permission not found or doesn't belong to the specified task");

            await taskPermissionRepository.DeleteAsync(permission);
            await taskPermissionRepository.SaveChangesAsync();

            return (ErrorType.Ok, "Permission deleted successfully");
        }
        public async Task<(ErrorType HttpCode, string Message, List<PermissionResp>? Permissions)> GetAllPermissionsForProject(int projectId)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return (ErrorType.Unauthorized, "Please login first", null);

            var user = await userRepository.GetByIdAsync(userId.Value);
            if (user == null)
                return (ErrorType.Unauthorized, "User does not exist", null);

            var project = await projectRepository.GetByIdAsync(projectId);
            if (project == null)
                return (ErrorType.NotFound, "Project not found", null);

            var assignee = await projectAssigneeRepository.GetByProjectAndUserIdAsync(projectId, userId.Value);
            if (assignee == null && userId != project.UserId)
                return (ErrorType.Unauthorized, "You are not authorized to view permissions for this project", null);

            if (assignee?.Role != "Admin" && userId != project.UserId)
                return (ErrorType.Unauthorized, "Only project owner or admin can view permissions", null);

            var permissions = await taskPermissionRepository.GetAllByProjectIdAsync(projectId);
            if(permissions.Count == 0)
                return (ErrorType.NotFound, "No permissions found for this project", null);

            List<PermissionResp> permissionResponse = (List<PermissionResp>)permissions.Select(p => new PermissionResp
            {
                Id = p.Id,
                UserEmail = p.ProjectAssignee.User.Email,
                TaskTitle = p.Task.Title,
                Permission = p.Permission.ToString(),

            }).ToList();
            return (ErrorType.Ok, "Permissions retrieved successfully", permissionResponse);
        }




    }
}
