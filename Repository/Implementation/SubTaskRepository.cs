using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.Models;
using TaskManagement.Repository.Interface;

namespace TaskManagement.Repository.Implementation
{
    public class SubTaskRepository : ISubTaskRepository
    {
        private readonly AppDbContext _context;

        public SubTaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SubTask>> GetByUserIdAsync(int userId)
        {
            return await _context.SubTasks
                .Include(s => s.Task)
                .Include(s => s.Logs)
                .Where(s => s.AssignedTo.UserId == userId)
                .ToListAsync();
        }
        public async Task<List<SubTask>> GetByTaskIdAsync(int taskId)
        {
            return await _context.SubTasks
                .Include(s => s.Task)
                .Include(s => s.AssignedTo)
                .Where(s => s.TaskId == taskId)
                .ToListAsync();
        }

        public async Task<SubTask?> GetByIdAsync(int id)
        {
            return await _context.SubTasks
                .Include(s => s.Task)
                .Include(s => s.AssignedTo)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(SubTask subTask)
        {
            await _context.SubTasks.AddAsync(subTask);
        }

        public async Task DeleteAsync(SubTask subTask)
        {
            _context.SubTasks.Remove(subTask);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
