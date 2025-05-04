using TaskManagement.Models;

namespace TaskManagement.Repository.Interface
{
    public interface ITaskPermissionRepository
    {
        Task<List<TaskPermission>> GetByUserAndTaskIdsAsync(int userId, int taskId);
        Task<List<TaskPermission>> GetByTaskIdAsync(int taskId);
        Task<TaskPermission?> GetByIdAsync(int id);
        Task<List<TaskPermission>> GetByUserIdAsync(int userId);

        Task AddAsync(TaskPermission permission);
        Task DeleteAsync(TaskPermission permission);
        Task SaveChangesAsync();
    }
}
