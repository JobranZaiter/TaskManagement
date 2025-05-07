using TaskManagement.Models;

namespace TaskManagement.Repository.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment> GetByIdAsync(int id);
        Task<List<Comment>> GetAllAsync();
        Task<List<Comment>> GetByTaskIdAsync(int taskId);
        Task<List<Comment>> GetByAssigneeIdAsync(int assigneeId);
        Task AddAsync(Comment comment);
        Task UpdateAsync(Comment comment);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}