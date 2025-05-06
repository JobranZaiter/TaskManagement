using TaskManagement.Models;

namespace TaskManagement.Repository.Interface
{
    public interface ISubTaskRepository
    {
        Task<List<SubTask>> GetByUserIdAsync(int userId);
        Task<SubTask?> GetByIdAsync(int id);
        Task<List<SubTask>> GetByTaskIdAsync(int taskId);
        Task AddAsync(SubTask subTask);
        Task DeleteAsync(SubTask subTask);
        Task SaveChangesAsync();
    }
}
