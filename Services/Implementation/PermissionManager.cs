using TaskManagement.Models.Enum;
using TaskManagement.Repository.Interface;
using TaskManagement.Services.Interface;

namespace TaskManagement.Services.Implementation
{
    public class PermissionManager : IPermissionManager
    {
        private readonly ITaskPermissionRepository taskPermissionRepo;
        public PermissionManager(ITaskPermissionRepository _taskPermissionRepository)
        {
            this.taskPermissionRepo = _taskPermissionRepository;
        }
        public async Task<bool> UserHasPermissionAsync(int userId, int taskId, PermissionType permission)
        {
            var permissions = await taskPermissionRepo.GetByUserAndTaskIdsAsync(userId, taskId);
            return permissions.Any(p => p.Permission == permission);
        }
    }
}
