using TaskManagement.Models;

namespace TaskManagement.Repository.Interface
{
    public interface IAppTaskRepository
    {
        Task<List<AppTask>> GetByUserIdAsync(int userId);
        Task<AppTask?> GetByIdAsync(int id);
        Task AddAsync(AppTask task);
        Task DeleteAsync(AppTask task);
        Task SaveChangesAsync();
    }
}
