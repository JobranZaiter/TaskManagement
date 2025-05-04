using System.IdentityModel.Tokens.Jwt;
using TaskManagement.Models;
using TaskManagement.Models.Enum;
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
        private readonly IPermissionManager permissionManager;
        public TaskService(IPermissionManager _permissionManager, IHttpContextAccessor _httpContextAccessor, ISubTaskRepository _subTaskRepository, IAppTaskRepository _appTaskRepository, IUserRepository _userRepository ) 
        {
            this.permissionManager = _permissionManager;
            this.httpContextAccessor = _httpContextAccessor;
            this.subTaskRepository = _subTaskRepository;
            this.taskRepository = _appTaskRepository;
            this.userRepository = _userRepository;
        }

        public async Task<(ErrorType HttpCode, string Message, List<SubTask>? Tasks)> GetAllSubTasks()
        {
            var id = httpContextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (id == null)
                return (ErrorType.Unauthorized, "Please login first.", null);
            int userId = int.Parse(id);
            if (!await userRepository.UserExistsAsync(userId))
                return (ErrorType.Unauthorized, "User does not exist", null);

            var subTasks = await subTaskRepository.GetByUserIdAsync(userId);
            if (subTasks == null || subTasks.Count == 0)
                return (ErrorType.NotFound, "No tasks found", null);

            return (ErrorType.Ok, "Tasks found", subTasks);
        }

        public async Task<(ErrorType HttpCode, string Message, List<AppTask>? Tasks)> GetAllProjectTasks(int projectId)
        {
            var id = httpContextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
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
    }
}
