using TaskManagement.Models;
namespace TaskManagement.Repository.Interface
{

    public interface IProjectRepository
    {
        Task<Project?> GetByIdAsync(int id);
        Task<List<Project>> GetAllByUserIdAsync(int userId);
        Task<List<Project>> GetAllAsync();
        Task<List<Project>> GetProjectsByAssigneeId(int id);
        Task AddAsync(Project project);
        Task DeleteAsync(Project project);
        Task<bool> ExistsAsync(int id);
        Task SaveChangesAsync();
    }

}
