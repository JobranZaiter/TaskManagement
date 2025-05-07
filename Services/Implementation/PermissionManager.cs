using TaskManagement.Models;
using TaskManagement.Models.Enum;
using TaskManagement.Repository.Interface;
using TaskManagement.Services.Interface;

namespace TaskManagement.Services.Implementation
{
    public class PermissionManager : IPermissionManager
    {
        private readonly ITaskPermissionRepository taskPermissionRepo;
        private readonly IUserRepository userRepository;
        private readonly IAppTaskRepository taskRepository;
        private readonly IProjectAssigneeRepository projectAssigneeRepository;
        public PermissionManager(IProjectAssigneeRepository _projectAssigneeRepository, IAppTaskRepository _taskRepository, ITaskPermissionRepository _taskPermissionRepository, IUserRepository _userRepository)
        {
            this.projectAssigneeRepository = _projectAssigneeRepository;
            this.taskRepository = _taskRepository;
            this.taskPermissionRepo = _taskPermissionRepository;
            this.userRepository = _userRepository;
        }
        public async Task<bool> UserHasPermissionAsync(int userId, int taskId, PermissionType permission)
        {
        
            User user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;
            AppTask task = await taskRepository.GetByIdAsync(taskId);
            if (task == null)
                return false;
            if(task.Project.UserId == userId)
                return true;
            var assignee = await projectAssigneeRepository.GetByProjectAndUserIdAsync(task.ProjectId, userId);
            if (assignee == null)
                return false;
            if (assignee.Role == "Admin")
                return true;

            var permissions = await taskPermissionRepo.GetByUserAndTaskIdsAsync(userId, taskId);
            return permissions.Any(p => p.Permission == permission);
        }
    }
}
