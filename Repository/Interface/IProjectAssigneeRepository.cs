using TaskManagement.Models;

namespace TaskManagement.Repository.Interface
{
    public interface IProjectAssigneeRepository
    {
        Task<ProjectAssignee?> GetByProjectAndUserIdAsync(int projectId, int userId);
        Task<ProjectAssignee?> GetByIdAsync(int id);
        Task<List<ProjectAssignee>> GetAllAsync();
        Task AddAsync(ProjectAssignee assignee);
        Task DeleteAsync(ProjectAssignee assignee);
        Task SaveChangesAsync();
    }
}
